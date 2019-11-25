using System.Collections;
using UnityEngine;

namespace PadTap.Maps
{
    [RequireComponent(typeof(Animator))]
    public class Indicator : MonoBehaviour
    {
        IndicatorSpawner spawner;
        Animator animator;

        private void Awake()
        {
            spawner = FindObjectOfType<IndicatorSpawner>();
            animator = GetComponent<Animator>();
            if(spawner == null)
            {
                Debug.LogError("No object with " + typeof(IndicatorSpawner) + " component found!\n");
            }
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
            animator.StartPlayback();
            if (lifespan > 0)
            {
                animator.speed = 1 / lifespan;
            }
        }

        private void GameOver()
        {
            spawner.GameOver();
        }

        public void Click()
        {
            if (spawner != null && TimeAlive() > spawner.map.threshold)
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