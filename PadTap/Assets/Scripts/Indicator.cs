using System.Collections;
using UnityEngine;

namespace PadTap
{
    public class Indicator : MonoBehaviour
    {
        [SerializeField] [Range(0, 5)] float lifespan = 2;

        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
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
            Debug.Log("lost");
        }

        public void Clicked()
        {
            Debug.Log("plus");
            Destroy(gameObject);
        }
    }
}