using UnityEngine;

namespace PadTap.Maps
{
    public class Threshold : MonoBehaviour
    {
        public void SetThreshold(float threshold)
        {
            //TODO change to child object
            transform.localScale = new Vector3(threshold, threshold, threshold);
        }

        public void SetPerfectScore(float perfectScore, float perfectScoreDifference)
        {
            //TODO
        }
    }
}