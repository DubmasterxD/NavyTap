using UnityEngine;

namespace PadTap
{
    public class Threshold : MonoBehaviour
    {
        [SerializeField] [Range(0, 1)] float threshold = .8f;

        private void Start()
        {
            SetThreshold();
        }

        private void SetThreshold()
        {
            transform.localScale = new Vector3(threshold, threshold, threshold);
        }
    }
}