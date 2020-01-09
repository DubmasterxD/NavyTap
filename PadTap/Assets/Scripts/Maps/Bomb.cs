using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
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
        gameObject.SetActive(false);
    }
}
