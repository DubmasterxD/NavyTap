using System.Collections;
using UnityEngine;

namespace NavyTap.Maps
{
    [RequireComponent(typeof(Animator))]
    public class Indicator : MonoBehaviour
    {
        IndicatorSpawner spawner;
        Animator animator;

        bool clicked = false;

        private void Awake()
        {
            spawner = FindObjectOfType<IndicatorSpawner>();
            animator = GetComponent<Animator>();
            if(spawner == null)
            {
                Debug.LogError(Logger.NoComponentFound(typeof(IndicatorSpawner)));
            }
        }

        public IEnumerator StartIndicatorIn(float time, float lifespan)
        {
            animator.speed = 0;
            yield return new WaitForSeconds(time);
            ChangeAnimatorSpeedFromLifespan(lifespan);
            yield return new WaitForSeconds(lifespan);
            if (clicked == false)
            {
                GameOver();
                Destroy(gameObject);
            }
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
            clicked = true;
            if (spawner != null && TimeAlive() > spawner.map.threshold)
            {
                if (GotPerfectScore())
                {
                    FindObjectOfType<Points>().AddPoints(100);
                }
                else
                {
                    FindObjectOfType<Points>().AddPoints(30);
                }
            }
            else
            {
                GameOver();
            }
            Destroy(gameObject);
        }

        private bool GotPerfectScore()
        {
            return TimeAlive() >= spawner.map.GetPerfectScore() - spawner.map.GetPerfectScoreAcceptableDifference() && TimeAlive() <= spawner.map.GetPerfectScore() + spawner.map.GetPerfectScoreAcceptableDifference();
        }

        public float TimeAlive()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }
}