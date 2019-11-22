using UnityEngine;

namespace PadTap.MapMaker
{
    public class IndicatorVisualizer : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer = null;
        private float indicatorSpeed = 1f;
        private float indicatorTime = 0f;

        public void ChangeSpeedFromFilespan(float lifespan)
        {
            indicatorSpeed = 1 / lifespan;
        }

        public void AnimateIndicator(float deltaTime)
        {
            if (spriteRenderer != null)
            {
                indicatorTime += deltaTime * indicatorSpeed;
                if (indicatorTime > 1)
                {
                    indicatorTime -= 1;
                }
                spriteRenderer.transform.localScale = new Vector3(indicatorTime, indicatorTime, indicatorTime);
            }
        }

        public void SetIndicatorScale(float lifeTime)
        {
            indicatorTime = lifeTime * indicatorSpeed;
            if (indicatorTime > 1 || indicatorTime < 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                AnimateIndicator(0);
            }
        }
    }
}