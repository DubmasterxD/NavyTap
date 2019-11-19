using UnityEngine;

namespace PadTap.Maps
{
    public class Threshold : MonoBehaviour
    {
        public void SetThreshold(float threshold)
        {
            transform.localScale = new Vector3(threshold, threshold, threshold);
        }
    }
}