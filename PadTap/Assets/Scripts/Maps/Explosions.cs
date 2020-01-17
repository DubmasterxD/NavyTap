using System.Collections.Generic;
using UnityEngine;

public class Explosions : MonoBehaviour
{
    [SerializeField] Explosion explosionPrefab = null;
    [SerializeField] float explosionsInPool = 5;

    List<Explosion> explosions = null;

    private void Start()
    {
        CreateExplosionsPool();
    }

    private void CreateExplosionsPool()
    {
        explosions = new List<Explosion>();
        for (int i = 0; i < explosionsInPool; i++)
        {
            explosions.Add(Instantiate(explosionPrefab, transform));
        }
    }

    public void MakeExplosion(Vector3 positionOfExplosion)
    {
        explosions[0].MakeExplosion(positionOfExplosion);
        explosions.Add(explosions[0]);
        explosions.RemoveAt(0);
    }
}
