using UnityEngine;

namespace PadTap {
    public class MapChoice : MonoBehaviour
    {
        [SerializeField] Map map = null;

        public void ChooseMap()
        {
            FindObjectOfType<Scene>().LoadMap(map);
        }
    }
}