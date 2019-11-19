using PadTap.Core;
using UnityEngine;

namespace PadTap.Menu
{
    public class MapChoice : MonoBehaviour
    {
        [SerializeField] Map map = null;

        public void ChooseMap()
        {
            FindObjectOfType<Scene>().LoadMap(map);
        }
    }
}