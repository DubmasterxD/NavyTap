using NavyTap.Maps;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    [SerializeField] float explosionLifespan = 0.2f;
    [SerializeField] Text tileRow = null;

    private float timer = 1000;

    Explosions explosions;
    BombSpawner bombSpawner;

    private void Awake()
    {
        explosions = FindObjectOfType<Explosions>();
        bombSpawner = FindObjectOfType<BombSpawner>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Explode();
        }
    }

    public void SetBombTimer(float time)
    {
        timer = time;
    }

    public void SetIndex(int index)
    {
        tileRow.text = index.ToString();
    }

    private void Explode()
    {
        explosions.MakeExplosion(transform.position);
        bombSpawner.SetNextBomb();
    }
}
