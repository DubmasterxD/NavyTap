using UnityEngine;
using UnityEngine.UI;

namespace NavyTap.Maps
{
    public class Timeline : MonoBehaviour
    {
        Image image;

        AudioSource audio;
        IndicatorSpawner spawner;

        private void Awake()
        {
            image = GetComponent<Image>();
            audio = FindObjectOfType<AudioSource>();
            spawner = FindObjectOfType<IndicatorSpawner>();
        }

        private void Update()
        {
            if (spawner != null && audio != null && image != null && spawner.map != null && spawner.map.song != null)
            {
                image.fillAmount = audio.time / spawner.map.song.length;
            }
            else
            {
                image.fillAmount = 0;
            }
        }
    }
}