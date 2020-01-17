using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float explosionLifespan = 0.2f;

    public void SetBombTimer(float time)
    {
        StartCoroutine(ExplodeIn(time));
    }

    private IEnumerator ExplodeIn(float time)
    {
        yield return new WaitForSeconds(time);
        Explode();
    }

    private void Explode()
    {
        FindObjectOfType<Explosions>().MakeExplosion(transform.position);
        Destroy(gameObject);
    }
}
