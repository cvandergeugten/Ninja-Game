using System;
using System.Collections;
using System.Collections.Generic;
using Ninja.Saving;
using UnityEngine;

namespace Ninja.Stats
{
    public class Honor : MonoBehaviour, ISaveable
    {
        [SerializeField] float honorPoints = 0;
        [SerializeField] float honorToVictory = 200f;

        public event Action onHonorGained;

        public void GainHonor(float honor)
        {
            honorPoints += honor;
            onHonorGained();
        }

        public float GetHonorToVictoryPercentage()
        {
            return honorPoints / honorToVictory;
        }

        public float GetPoints()
        {
            return honorPoints;
        }

        public object CaptureState()
        {
            return honorPoints;
        }

        public void RestoreState(object state)
        {
            honorPoints = (float)state;
        }
    }
}
