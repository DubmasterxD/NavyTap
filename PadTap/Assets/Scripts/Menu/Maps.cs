using NavyTap.Core;
using UnityEngine;

namespace NavyTap.Menu
{
    public class Maps : MonoBehaviour
    {
        [SerializeField] Map[] playableMaps = null;
        [SerializeField] MapChoice mapChoicePrefab = null;

        private void Start()
        {
            foreach(Map map in playableMaps)
            {
                MapChoice newChoice = Instantiate(mapChoicePrefab, transform);
                newChoice.SetMap(map);
            }
        }
    }
}