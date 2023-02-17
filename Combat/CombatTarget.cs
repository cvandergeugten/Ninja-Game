using System.Collections;
using System.Collections.Generic;
using Ninja.Attribues;
using Ninja.Control;
using UnityEngine;

namespace Ninja.Combat
{
    // Requires the object to contain the Health script
    [RequireComponent(typeof(Health))]
    
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }

            //Don't let the player target themselves with an attack
            if (Input.GetMouseButton(0) && !this.CompareTag("Player"))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
    }
}
