using UnityEngine;
using System.Collections;

public class FurbyAnimatorScript : MonoBehaviour {

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
}
