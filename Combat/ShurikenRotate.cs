using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninja.Combat
{
    public class ShurikenRotate : MonoBehaviour
    {
        [SerializeField] float rotationSpeed = 100f;
        void Update()
        {
            transform.Rotate(new Vector3(0f, rotationSpeed, 0f) * Time.deltaTime);
        }
    }
}
