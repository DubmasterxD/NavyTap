using UnityEngine;

namespace NavyTap.Maps
{
    public class Taps : MonoBehaviour
    {
        private void Update()
        {
            Click();
            //Touch();
        }

        private void Click()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(GetMousePosition(), Camera.main.transform.forward, 20f);
                Tapped(hits);
            }
        }

        private void Touch()
        {
            for(int i=0; i < Input.touchCount; i++)
            {
                if(Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    RaycastHit2D[] hits = Physics2D.RaycastAll(GetTouchPosition(Input.GetTouch(i)), Camera.main.transform.forward, 20f);
                    Tapped(hits);
                }
            }
        }

        private Vector3 GetTouchPosition(Touch touch)
        {
            return Camera.main.ScreenToWorldPoint(touch.position);
        }

        private void Tapped(RaycastHit2D[] hits)
        {
            Indicator indicatorToClick = GetIndicatorHit(hits);
            if (indicatorToClick != null)
            {
                indicatorToClick.Click();
            }
            Tile tileClicked = GetTileHit(hits);
            if (tileClicked != null)
            {
                VisualizationButtons buttons = FindObjectOfType<VisualizationButtons>();
                if (buttons != null)
                {
                    buttons.TileClicked(tileClicked.tileIndex);
                }
            }
        }

        private Tile GetTileHit(RaycastHit2D[] hits)
        {
            Tile tileClicked = null;
            foreach (RaycastHit2D hit in hits)
            {
                Tile tileHit = hit.collider.gameObject.GetComponent<Tile>();
                if (tileHit != null)
                {
                    tileClicked = tileHit;
                }
            }
            return tileClicked;
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