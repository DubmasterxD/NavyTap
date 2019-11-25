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
            try
            {
                scene.LoadMap(map);
            }
            catch (System.Exception e)
            {
                Debug.LogError("No object with " + typeof(Scene) + " component found!\n" + e);
            }
        }
    }
}