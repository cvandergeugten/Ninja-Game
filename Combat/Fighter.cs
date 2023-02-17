using Ninja.Attribues;
using Ninja.Core;
using Ninja.Movement;
using UnityEngine;
using GameDevTV.Utils;
using Ninja.Stats;
using System.Collections.Generic;
using Ninja.Saving;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ninja.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] List<WeaponConfig> weaponList = null;
        [SerializeField] UnityEvent onSwap;


        private KeyCode[] weaponKeys;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        private GameObject hitEffect;
        private GameObject[] weaponSelectionUI;


        private void Awake()
        {
            weaponKeys = new KeyCode[] {KeyCode.Alpha1,KeyCode.Alpha2,KeyCode.Alpha3};
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            weaponSelectionUI = GameObject.FindGameObjectsWithTag("Selector");
        }

        

        void Start()
        {
            currentWeapon.ForceInit();
        }

   
        void Update()
        {   
            
            timeSinceLastAttack += Time.deltaTime;
            SwapWeapon();

            

            // Set faster animation speed for thrown weapons
            if (currentWeaponConfig.HasProjectile())
            {
                GetComponent<Animator>().SetFloat("animationSpeedMultiplier", 2.5f);
            }
            else
            {
                GetComponent<Animator>().SetFloat("animationSpeedMultiplier", 1);
            }

            if (target == null) return;
            if (target.IsDead()) return;


            
            // Attack Behaviour
            if (!GetIsInRange(target.transform))
            {
                StopDance();
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                StopDance();
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void SwapWeapon()
        {
            //Only allow this functionality for the player
            if (gameObject != GameObject.FindGameObjectWithTag("Player")) return;

            //Disable functionality if player is dead
            if (gameObject.GetComponent<Health>().IsDead() == true) return;

            //Equip weapon based on which key is pressed
            for (int i = 0; i < weaponKeys.Length; i++)
            {
                if (Input.GetKeyDown(weaponKeys[i]))
                {
                    GetComponent<ActionScheduler>().CancelCurrentAction();
                    EquipWeapon(weaponList[i]);

                    //Update weapon selection UI
                    foreach (GameObject weaponSelection in weaponSelectionUI)
                    {
                        weaponSelection.GetComponent<Image>().enabled = false;
                    }
                    weaponSelectionUI[i].GetComponent<Image>().enabled = true;
                }
            }

            
        }


        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) &&
                !GetIsInRange(combatTarget.transform))
            {
                return false;
            }

            // Found a target that is in range and can move to. Check if target is alive
            // and can be attacked
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        public Health GetTarget()
        {
            return target;
        }
        

        private void AttackBehaviour()
        {
            // Look at the target when attacking
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            onSwap.Invoke();
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);

        }

        private void TriggerDance()
        {
            Animator animator = GetComponent<Animator>();

            animator.ResetTrigger("stopDance");
            animator.SetTrigger("dance");
        }

        private void StopDance()
        {
            Animator animator = GetComponent<Animator>();

            animator.ResetTrigger("dance");
            animator.SetTrigger("stopDance");
        }

        private void TriggerAttack()
        {
            Animator animator = GetComponent<Animator>();
            //currentWeaponConfig.SetAnimatorOverride(animator);

            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }

        private void StopAttack()
        {
            Animator animator = GetComponent<Animator>();
            
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

        // Animation Events Start
        void Hit()
        {
            
            if (target == null) return;
            hitEffect = currentWeaponConfig.GetRandomHitEffect();


            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }


            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
                // Trigger hit effect for current weapon
                if (hitEffect != null)
                {
                    // Produce random variation in direction of the hit effect (mostly for blood)
                    float bloodVariation = Random.Range(-40f,40f);
                    Transform enemyLocation = target.GetComponent<Transform>();
                    Vector3 rotate = enemyLocation.rotation.eulerAngles;
                    rotate = new Vector3(rotate.x,rotate.y+90+bloodVariation,rotate.z);

                    Instantiate(hitEffect, enemyLocation.position, Quaternion.Euler(rotate));
                }

            }
            
        }

        void FootR()
        {

        }

        void FootL()
        {

        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetRange();
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
