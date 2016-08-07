using UnityEngine;
using System.Collections;

public class FurbyAnimatorScript : MonoBehaviour {

    //public Sprite upSprite;
    //public Sprite downSprite;
    //public Sprite rightSprite;
    public Sprite leftSprite;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

	void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        leftSprite = spriteRenderer.sprite;
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

    public void PlayWalkAnimationIfWalking(Vector3 walk)
    {
        if (!IsZeroVector(walk))
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            

            animator.SetBool("IsWalking", false);
            return;
        }

        if (IsRight(walk))
        {
            //SetSprite(rightSprite);

            animator.SetBool("WalkRight", true);

            spriteRenderer.flipX = true;
        }

        if (IsLeft(walk))
        {
            SetSprite(leftSprite);

            animator.SetBool("WalkRight", false);

            spriteRenderer.flipY = false;
        }
        /*
        if (IsUp(walk))
            SetSprite(upSprite);

        if (IsDown(walk))
            SetSprite(downSprite);
            */
    }

    private void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }    

    private bool IsZeroVector(Vector3 vec)
    {
        return vec.x == 0f && vec.z == 0f;
    }

    private bool IsUp(Vector3 vec)
    {
        return vec.z > 0;
    }

    private bool IsDown(Vector3 vec)
    {
        return vec.z < 0;
    }

    private bool IsRight(Vector3 vec)
    {
        return vec.x > 0;
    }

    private bool IsLeft(Vector3 vec)
    {
        return vec.x < 0;
    }
}
