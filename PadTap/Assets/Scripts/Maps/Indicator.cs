using System.Collections;
using UnityEngine;

namespace PadTap.Maps
{
    public class Indicator : MonoBehaviour
    {
        IndicatorSpawner spawner;
        Animator animator;

        [SerializeField] bool isDestroyable = true;
        [SerializeField] bool isClickable = true;

        private void Awake()
        {
            spawner = FindObjectOfType<IndicatorSpawner>();
            animator = GetComponent<Animator>();
        }

        public void StartIndicator(float lifespan)
        {
            if (isDestroyable)
            {
                StartCoroutine(AutoDestroyIn(lifespan));
            }
            ChangeAnimatorSpeedFromLifespan(lifespan);
        }

        IEnumerator AutoDestroyIn(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            GameOver();
            Destroy(gameObject);
        }

        public void ChangeAnimatorSpeedFromLifespan(float lifespan)
        {
            if (animator != null)
            {
                animator.StartPlayback();
                animator.speed = 1 / lifespan;
            }
        }

        private void GameOver()
        {
            FindObjectOfType<IndicatorSpawner>().GameOver();
        }

        public void Click()
        {
            if (isClickable)
            {
                if (TimeAlive() > spawner.Map.threshold)
                {
                    //TODO Points
                }
                else
                {
                    GameOver();
                }
                Destroy(gameObject);
            }
        }

        public float TimeAlive()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }
}