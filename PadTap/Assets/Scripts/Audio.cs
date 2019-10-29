using UnityEngine;

namespace PadTap
{
    public class Audio : MonoBehaviour
    {
        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Play(AudioClip song)
        {
            audioSource.clip = song;
            audioSource.Play();
        }
    }
}