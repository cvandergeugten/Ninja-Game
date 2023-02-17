using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninja.Core
{
    public class BackgroundAudio : MonoBehaviour
    {
        private AudioSource audioSource;
        public AudioClip[] songs;
        public float volume;

        private void Start() 
        {
            audioSource = GetComponent<AudioSource>();

            if (!audioSource.isPlaying)
            {
                ChangeSong(Random.Range(0, songs.Length));
            }
        }

        // Update is called once per frame
        void Update()
        {
            audioSource.volume = volume;

            if (!audioSource.isPlaying)
            {
                ChangeSong(Random.Range(0, songs.Length));
            }
        }

        public void ChangeSong(int songPicked)
        {
            audioSource.clip = songs[songPicked];
            audioSource.Play();
        }
    }
}
