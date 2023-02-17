using System.Collections;
using System.Collections.Generic;
using Ninja.Attribues;
using UnityEngine;
using UnityEngine.Events;

namespace Ninja.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent onHit;
        [SerializeField] UnityEvent onStartAttack;


        public void OnHit()
        {
            onHit.Invoke();
        }

        public void OnStartAttack()
        {
            onStartAttack.Invoke();
        }
    }
}
