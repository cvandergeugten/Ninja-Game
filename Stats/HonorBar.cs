using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninja.Stats
{
    public class HonorBar : MonoBehaviour
    {
        [SerializeField] RectTransform foreground = null;

        GameObject player;
        Honor honorComponent;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            honorComponent = player.GetComponent<Honor>();
        }

        void Update()
        {
            foreground.localScale = new Vector3(honorComponent.GetHonorToVictoryPercentage(), 1, 1);
        }
    }
}
