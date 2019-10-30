using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioClip song { get; set; }

    float time = 0;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        song = audioSource.clip;
    }

    private void Update()
    {
        if (audioSource.isPlaying)
        {
            time = audioSource.time;
        }
    }

    public void ChooseSong(AudioClip newSong)
    {
        song = newSong;
    }

    public void Play()
    {
        if (!IsPlaying())
        {
            ChangeTime(time);
            audioSource.Play();
        }
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    public void Stop()
    {
        time = audioSource.time;
        audioSource.Stop();
    }

    public void ChangeTime(float time)
    {
        if (!IsPlaying())
        {
            this.time = time;
            audioSource.time = time;
        }
    }

    public float GetTime()
    {
        return time;
    }
}
