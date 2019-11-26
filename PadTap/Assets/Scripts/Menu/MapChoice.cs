using PadTap.Core;
using UnityEngine;

namespace PadTap.Menu
{
    public class MapChoice : MonoBehaviour
    {
        [SerializeField] Map map = null;

        public void ChooseMap()
        {
            Scene scene = FindObjectOfType<Scene>();
            if (scene != null)
            {
                scene.LoadMap(map);
            }
            else
            {
                Logger.NoComponentFound(typeof(Scene));
            }
        }
    }
}