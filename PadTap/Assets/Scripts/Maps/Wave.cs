using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] private float waveSpeedMultiplier = 1;
    [SerializeField] private Transform wave1 = null;
    [SerializeField] private Transform wave2 = null;

    const float waveWidth = 20.48f;
    Vector3 waveSpawnPosition = new Vector3(-waveWidth, 0, 0);

    private void Start()
    {
        wave1.localPosition = Vector3.zero;
        wave2.localPosition = waveSpawnPosition;
    }

    void Update()
    {
        WavesFlow();
    }

    private void WavesFlow()
    {
        float waveSpeed = Time.deltaTime * waveSpeedMultiplier * transform.localScale.x;
        wave1.transform.position += new Vector3(waveSpeed, 0, 0);
        wave2.transform.position += new Vector3(waveSpeed, 0, 0);
        if (wave1.localPosition.x >= waveWidth)
        {
            wave1.localPosition = waveSpawnPosition + new Vector3(wave2.localPosition.x, 0, 0);
        }
        if (wave2.localPosition.x >= waveWidth)
        {
            wave2.localPosition = waveSpawnPosition + new Vector3(wave1.localPosition.x, 0, 0);
        }
    }
}
