using UnityEngine;

namespace PadTap.Maps
{
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
                RaycastHit2D[] hits = Physics2D.RaycastAll(GetMousePosition(), Camera.main.transform.forward, 20f);
                Indicator indicatorToClick = GetIndicatorHit(hits);
                if (indicatorToClick != null)
                {
                    indicatorToClick.Click();
                }
            }
        }

        private Indicator GetIndicatorHit(RaycastHit2D[] hits)
        {
            Indicator indicatorToClick = null;
            float oldestIndicator = 0f;
            foreach (RaycastHit2D hit in hits)
            {
                Indicator indicatorHit = hit.collider.gameObject.GetComponent<Indicator>();
                if (indicatorHit != null && indicatorHit.TimeAlive() > oldestIndicator)
                {
                    oldestIndicator = indicatorHit.TimeAlive();
                    indicatorToClick = indicatorHit;
                }
            }
            return indicatorToClick;
        }

        private static Vector3 GetMousePosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}