using UnityEngine;
using UnityEngine.UI;

public class SongSlider : MonoBehaviour
{
    Slider slider;
    Audio audio;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        audio = FindObjectOfType<Audio>();
    }

    private void Start()
    {
        SetMinMax();
    }

    private void Update()
    {
        SetValue();
    }

    public void SetValue()
    {
        if (audio.IsPlaying())
        {
            slider.value = audio.GetTime();
        }
    }

    public void SetMinMax()
    {
        slider.minValue = 0;
        slider.maxValue = audio.song.length;
    }
}
