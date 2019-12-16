using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PadTap.MapMaker
{
    public class Waveform : MonoBehaviour
    {
        [SerializeField] LineRenderer lineRenderer = null;
        float startingVerticalScale = 1;
        Coroutine loadingMap;

        public void VerticalZoomIn()
        {
            if (transform.localScale.y / startingVerticalScale < Mathf.Pow(2, 3))
            {
                transform.localScale = new Vector3(1, transform.localScale.y * 2, 1);
                float tmp = (transform.localPosition.y + 0.5f) / -0.5f;
                for (int i = 0; i < tmp; i++)
                {
                    MoveUp();
                }
            }
        }

        public void VerticalZoomOut()
        {
            if (transform.localScale.y != startingVerticalScale)
            {
                transform.localScale = new Vector3(1, transform.localScale.y / 2, 1);
                float tmp = (transform.localPosition.y + 0.5f) / -0.5f / 2;
                for (int i = 0; i < tmp; i++)
                {
                    MoveDown();
                }
            }
        }

        public void CreateAudioWaveform(AudioClip song)
        {
            if (loadingMap != null)
            {
                StopCoroutine(loadingMap);
            }
            loadingMap = StartCoroutine(CreateAudioWaveformc(song));
        }

        private IEnumerator CreateAudioWaveformc(AudioClip song)
        {
            if (song != null)
            {
                int samples = (int)(song.frequency * song.length) * 2;
                float[] data = new float[samples];
                song.GetData(data, 0);
                float sum = 0;
                lineRenderer.positionCount = 0;
                List<Vector3> dataa = new List<Vector3>();
                float max = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    sum += Mathf.Abs(data[i]);
                    if (i % 200 == 0)
                    {
                        dataa.Add(new Vector3(i / (float)song.frequency * 5 / song.length, sum / 200, 0));
                        sum = 0;
                        lineRenderer.positionCount += 1;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, dataa[lineRenderer.positionCount - 1]);
                        if (dataa[lineRenderer.positionCount - 1].y > max)
                        {
                            max = dataa[lineRenderer.positionCount - 1].y;
                            transform.localScale = new Vector3(1, 1 / max, 1);
                        }
                        if (i % 100000 == 0)
                        {
                            yield return null;
                        }
                    }
                }
                transform.localScale = new Vector3(1, 1 / max, 1);
                startingVerticalScale = 1 / max;
            }
            else
            {
                Logger.ReceivedNull(typeof(AudioClip));
            }
        }

        public void MoveUp()
        {
            if (transform.localPosition.y > -0.5f - 0.5f * transform.localScale.y / startingVerticalScale * 2 + 0.5f)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 1 / 2f, transform.localPosition.z);
            }
        }

        public void MoveDown()
        {
            if (transform.localPosition.y != -0.5)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1 / 2f, transform.localPosition.z);
            }
        }

        public void ResetZoom()
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -0.5f, 0);
            transform.localScale = new Vector3(1, startingVerticalScale, 1);
        }
    }
}
