using System.Collections;
using UnityEngine;

namespace PadTap
{
    public class Indicator : MonoBehaviour
    {
        Spawner spawner;
        Animator animator;

        [SerializeField] bool isDestroyable = true;
        [SerializeField] bool isClickable = true;

        private void Awake()
        {
            spawner = FindObjectOfType<Spawner>();
            animator = GetComponent<Animator>();
        }

        public void StartIndicator(float lifespan)
        {
            if (isDestroyable)
            {
                StartCoroutine(AutoDestroy(lifespan));
            }
            ChangeAnimatorSpeedFromLifespan(lifespan);
        }

        IEnumerator AutoDestroy(float seconds)
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
            FindObjectOfType<Spawner>().GameOver();
            Debug.Log("lost");
        }

        public void Click()
        {
            if (isClickable)
            {
                if (TimeAlive() > spawner.Map.threshold)
                {
                    Debug.Log("plus");
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