using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using Ninja.Core;
using Ninja.Stats;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Ninja.Attribues
{
    public class Health : MonoBehaviour
    {
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [Range(0,1)]
        [SerializeField] float healthPickupDropChance;

        [SerializeField] GameObject healthDropObject;
        public AudioClip[] dieSounds;
        public AudioClip healthRestoreSound;
        private AudioSource source;
        


        LazyValue<float> healthPoints;

        bool isDead = false;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        private void Awake() 
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        void Start()
        {
            healthPoints.ForceInit();
            source = GetComponent<AudioSource>();
        }

   
        void Update()
        {

        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);


            if (healthPoints.value == 0)
            {
                onDie.Invoke();
                Die();
                AwardHonor(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public void Heal(float healthToRestore)
        {
            AudioClip temp = source.clip;
            source.clip = healthRestoreSound;
            source.PlayOneShot(source.clip);
            source.clip = temp;
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());       
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;

            AudioClip temp = source.clip;
            source.clip = dieSounds[Random.Range(0, dieSounds.Length)];
            source.PlayOneShot(source.clip);
            source.clip = temp;



            //Chance to spawn health pickup
            if (gameObject.tag == "Enemy")
            {
                float randomDropValue = Random.value;
                if (randomDropValue <= healthPickupDropChance)
                {
                    Instantiate(healthDropObject, gameObject.transform.position, gameObject.transform.rotation);
                }
            }

            // Steps for smooth death animation 
            GetComponent<Animator>().applyRootMotion = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<NavMeshAgent>().enabled = false;
        }

        private void AwardHonor(GameObject instigator)
        {
            Honor honor = instigator.GetComponent<Honor>();
            if (honor == null) return;

            honor.GainHonor(GetComponent<BaseStats>().GetStat(Stat.HonorReward));
        }
    }
}
