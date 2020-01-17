using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticles = null;
    Animation anim;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public void MakeExplosion(Vector3 positionOfExplosion)
    {
        transform.position = positionOfExplosion;
        anim.Play();
        explosionParticles.Play();
    }
}
