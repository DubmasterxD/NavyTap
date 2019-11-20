using System.Collections;
using UnityEngine;

namespace PadTap.Maps
{
    public class Indicator : MonoBehaviour
    {
        IndicatorSpawner spawner;
        Animator animator;

        private void Awake()
        {
            spawner = FindObjectOfType<IndicatorSpawner>();
            animator = GetComponent<Animator>();
        }

        public void StartIndicator(float lifespan)
        {
            StartCoroutine(AutoDestroyIn(lifespan));
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
            spawner.GameOver();
        }

        public void Click()
        {
            if (TimeAlive() > spawner.map.threshold)
            {
                //TODO Points
            }
            else
            {
                GameOver();
            }
            Destroy(gameObject);
        }

        public float TimeAlive()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }
}