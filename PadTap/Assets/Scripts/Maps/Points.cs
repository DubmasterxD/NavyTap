using UnityEngine;
using UnityEngine.UI;

namespace PadTap.Maps
{
    public class Points : MonoBehaviour
    {
        Text text;
        int points;

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        public void AddPoints(int newPoints)
        {
            points += newPoints;
            text.text = points.ToString();
        }
    }
}