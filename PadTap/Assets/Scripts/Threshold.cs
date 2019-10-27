using UnityEngine;

namespace PadTap
{
    public class Threshold : MonoBehaviour
    {
        public void SetThreshold(float threshold)
        {
            transform.localScale = new Vector3(threshold, threshold, threshold);
        }
    }
}