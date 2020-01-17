using UnityEngine;

public class TapVisualizationButton : MonoBehaviour
{
    Animation anim;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public void LightButton()
    {
        anim.Play();
    }
}
