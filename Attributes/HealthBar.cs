using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ninja.Attribues
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] RectTransform foreground = null;

        GameObject player;
        Health healthComponent;

        private void Awake() 
        {
            player = GameObject.FindGameObjectWithTag("Player");
            healthComponent = player.GetComponent<Health>();
        }

        void Update()
        {
            foreground.localScale = new Vector3(healthComponent.GetFraction(),1,1);
        }
    }
}
