using NavyTap.Core;
using UnityEngine;

namespace NavyTap.Maps
{
    [RequireComponent(typeof(AudioSource))]
    public class Audio : MonoBehaviour
    {
        [SerializeField] float acceptableTimeDifference = 0.001f;

        AudioSource audioSource;
        GameManager game;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            game = FindObjectOfType<GameManager>();
        }

        private void Update()
        {
            if(game.isPlaying && game.currentTime - game.GetChosenMap().indicatorLifespan < audioSource.time-acceptableTimeDifference)
            {
                audioSource.time = game.currentTime - game.GetChosenMap().indicatorLifespan;
            }
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