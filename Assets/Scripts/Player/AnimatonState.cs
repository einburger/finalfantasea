using UnityEngine;

namespace Fishnet {

public class AnimationChanger
{
    // Start is called before the first frame update
    Animator animator;

    public AnimationChanger(Animator _animator) {
        animator = _animator;
    }
    public void SetIdle() {
        animator.SetBool("idle", true);
        animator.SetBool("walking", false);
        animator.SetBool("sailing", false);
    }

    public void SetWalking() {
        animator.SetBool("idle", false);
        animator.SetBool("walking", true);
        animator.SetBool("sailing", false);
    }

    public void SetSailing() {
        animator.SetBool("aiming", false);
        animator.SetBool("walking", false);
        animator.SetBool("idle", false);
        animator.SetBool("sailing", true);
    }

    public void SetAiming() {
        animator.SetBool("aiming", true);
    }

    public void UnsetAiming() {
        animator.SetBool("aiming", false);
    }

    public void SetCasting() {
        animator.SetBool("casting", true);
    }

}

}