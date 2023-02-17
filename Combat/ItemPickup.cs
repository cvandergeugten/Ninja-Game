using System.Collections;
using System.Collections.Generic;
using Ninja.Attribues;
using Ninja.Control;
using Ninja.Movement;
using UnityEngine;

namespace Ninja.Combat
{
    public class ItemPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] float respawnTime = 5;
        [SerializeField] float healthToRestore = 0;

        GameObject player;

        private void Awake() 
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && !player.GetComponent<Health>().IsDead())
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }

            if (respawnTime != 0)
            {
                StartCoroutine(HideForSeconds(respawnTime));
            }
            else
            {
                StartCoroutine(HideForSeconds(5));
                Destroy(gameObject);
            }
            
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
        

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Mover>().MoveTo(gameObject.transform.position, 1f);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
