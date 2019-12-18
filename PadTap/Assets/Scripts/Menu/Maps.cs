using PadTap.Core;
using UnityEngine;

namespace PadTap.Menu
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