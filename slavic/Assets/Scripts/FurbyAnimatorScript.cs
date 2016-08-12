using UnityEngine;
using System.Collections;

public class FurbyAnimatorScript : MonoBehaviour {

    //public Sprite upSprite;
    //public Sprite downSprite;
    //public Sprite rightSprite;
    public Sprite leftSprite;

    public int numberOfDirectionChangesPerSec = 16;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float directionChangeTimer;

	void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        leftSprite = spriteRenderer.sprite;

        directionChangeTimer = 0f;
    }        

    void Update()
    {
        if(directionChangeTimer > 0)
        {
            directionChangeTimer -= Time.deltaTime;
        }
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

    public void PlayWalkingAnimation(Vector3 walk)
    {
        if (!IsZeroVector(walk))
        {
            animator.SetBool("IsWalking", true);

            if (directionChangeTimer <= 0f)
            {                
                if (IsRight(walk) && !animator.GetBool("WalkRight"))
                {
                    //SetSprite(rightSprite);

                    SetFurbyDirection(true);
                }

                if (IsLeft(walk) && animator.GetBool("WalkRight"))
                {
                    SetSprite(leftSprite);
                    SetFurbyDirection(false);
                }
            }
        }
        
        /*
        if (IsUp(walk))
            SetSprite(upSprite);

        if (IsDown(walk))
            SetSprite(downSprite);
            */
    }

    public void StopWalkingAnimation()
    {
        animator.SetBool("IsWalking", false);
    }

    private void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    } 
    
    private void SetFurbyDirection(bool IsRight)
    {
        animator.SetBool("WalkRight", IsRight);
        spriteRenderer.flipX = IsRight;
        directionChangeTimer += (1f / numberOfDirectionChangesPerSec);
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
