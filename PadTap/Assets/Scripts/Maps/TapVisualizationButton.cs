using UnityEngine;

public class TapVisualizationButton : MonoBehaviour
{
    [SerializeField] float blinkAnimationTime = .2f;
    Animator anim;
    int _clickedAnimatorTrigger = Animator.StringToHash("Clicked");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        anim.speed = 1/blinkAnimationTime;
    }

    public void LightButton()
    {
        anim.SetTrigger(_clickedAnimatorTrigger);
    }
}
