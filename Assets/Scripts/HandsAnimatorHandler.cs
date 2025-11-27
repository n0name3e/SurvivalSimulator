using UnityEngine;

public class HandsAnimatorHandler : MonoBehaviour
{
    private Animator animator;

    public void PlayHandAnimation(string animationName)
    {
        if (animator != null)
        {
            animator.Play(animationName);
        }
    }
    // called in animation in certain frame
    public void Hit()
    {
        Inventory.instance.HitWithTool();
        print("Hit called from animation");
    }
}
