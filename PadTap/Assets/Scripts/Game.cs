using UnityEngine;

namespace PadTap
{
    public class Game : MonoBehaviour
    {
        public Map chosenMap { get; set; }

        private void Awake()
        {
            if (FindObjectsOfType<Game>().Length == 1)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}