using System.Collections;
using UnityEngine;

namespace PadTap
{
    public class Indicator : MonoBehaviour
    {
        Spawner spawner;
        Animator animator;

        private void Awake()
        {
            spawner = FindObjectOfType<Spawner>();
            animator = GetComponent<Animator>();
        }

        public void StartIndicator(float lifespan)
        {
            StartCoroutine(AutoDestroy(lifespan));
            animator.speed = 1 / lifespan;
        }

        IEnumerator AutoDestroy(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            GameOver();
            Destroy(gameObject);
        }

        private void GameOver()
        {
            FindObjectOfType<Spawner>().GameOver();
            Debug.Log("lost");
        }

        public void Click()
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

        public float TimeAlive()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }
}