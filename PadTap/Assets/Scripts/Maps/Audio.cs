using UnityEngine;

namespace PadTap.Maps
{
    [RequireComponent(typeof(AudioSource))]
    public class Audio : MonoBehaviour
    {
        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Play(AudioClip song)
        {
            if (song != null)
            {
                audioSource.clip = song;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning(typeof(AudioClip) + " received is null");
            }
        }
    }
}