using UnityEngine;

namespace PadTap {
    public class Taps : MonoBehaviour
    {
        private void Update()
        {
            Tap();
        }

        private void Tap()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, 20f);
                foreach (RaycastHit2D hit in hits)
                {
                    Indicator indicatorHit = hit.collider.gameObject.GetComponent<Indicator>();
                    if (indicatorHit != null)
                    {
                        indicatorHit.Clicked();
                    }
                }
            }
        }
    }
}