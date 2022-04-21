using UnityEngine;

public class Dialog_Animation 
{
    private Animator animator;
    private int hashAppear;
    private int hashDisappear;

    public Dialog_Animation(Animator _animator)
    {
        animator = _animator;
        hashAppear = Animator.StringToHash("Appear");
        hashDisappear = Animator.StringToHash("Disappear");
    }

    public void Appear()
    {
        animator.SetTrigger(hashAppear);
    }

    public void Disappear()
    {
        animator.SetTrigger(hashDisappear);
    }

}
