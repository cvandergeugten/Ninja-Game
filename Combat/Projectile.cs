using System.Collections;
using System.Collections.Generic;
using Ninja.Attribues;
using UnityEngine;
using UnityEngine.Events;

namespace Ninja.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject[] hitEffects = null;
        [SerializeField] float maxLifeTime = 7;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;
        [SerializeField] UnityEvent onHit;
        [SerializeField] UnityEvent onLaunch;


        Health target = null;
        GameObject instigator = null;
        float damage = 0;

        private void Start()
        {
            onLaunch.Invoke();
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }


        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            target.TakeDamage(instigator, damage);

            speed = 0;

            onHit.Invoke();

            if (hitEffects != null)
            {
                float bloodVariation = Random.Range(-40f, 40f);
                Transform enemyLocation = target.GetComponent<Transform>();
                Vector3 rotate = enemyLocation.rotation.eulerAngles;
                rotate = new Vector3(rotate.x, rotate.y + 90 + bloodVariation, rotate.z);

                Instantiate(hitEffects[Random.Range(0, hitEffects.Length)], enemyLocation.position, Quaternion.Euler(rotate));
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
