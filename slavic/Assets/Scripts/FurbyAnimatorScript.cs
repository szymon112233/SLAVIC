using UnityEngine;
using System.Collections;

public class FurbyAnimatorScript : MonoBehaviour {

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite rightSprite;
    public Sprite leftSprite;

    private Animator animator;

	void Awake()
    {
        animator = GetComponent<Animator>();
    }        

	public void PlayDeathAnimation()
    {
        animator.SetTrigger("Die");
    }

    public void PlayJumpAnimation()
    {
        animator.SetTrigger("Jump");
    }

    public void PlayHurtAnimation()
    {
        animator.SetTrigger("Hurt");
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }
}
