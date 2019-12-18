using PadTap.Core;
using UnityEngine;
using UnityEngine.UI;

namespace PadTap.Menu
{
    public class MapChoice : MonoBehaviour
    {
        [SerializeField] Text mapName = null;

        Map map = null;

        public void SetMap(Map newMap)
        {
            map = newMap;
            ActualizeMapInfo();
        }

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

        private void ActualizeMapInfo()
        {
            if (map != null)
            {
                mapName.text = map.mapName;
            }
        }
    }
}