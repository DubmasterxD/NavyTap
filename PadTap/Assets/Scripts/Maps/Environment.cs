using UnityEngine;

namespace PadTap.Maps
{
    public class Environment : MonoBehaviour
    {
        [SerializeField] private float heightDifference = 1;
        [SerializeField] private float shipWaveSpeed = 1;
        [SerializeField] private float sideDifference = 1;

        private float startingHeight = 0;
        private float angle = 0;

        private void Start()
        {
            startingHeight = transform.position.y;
        }

        void Update()
        {
            WaveShip();
        }

        private void WaveShip()
        {
            angle += Time.deltaTime * shipWaveSpeed;
            if (angle > Mathf.PI * 2)
            {
                angle -= Mathf.PI * 2;
            }
            transform.position = new Vector3(0, startingHeight - Mathf.Sin(angle) * heightDifference, 0);
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(angle) * sideDifference);
        }
    }
}