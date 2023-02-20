using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator mainAnimator;

    // Start is called before the first frame update
    void Awake()
    {
        mainAnimator = GetComponent<Animator>();
    }

    public void AttackAnim(bool isAttacking)
    {
        mainAnimator.SetBool("isAttacking", isAttacking);
    }

    public void ShootAnim(bool shoot)
    {
        mainAnimator.SetBool("shoot", shoot);
    }

    public void MovementAnim(float isMoving)
    {
        mainAnimator.SetFloat("isMoving", isMoving);
    }

    public void DyingAnim()
    {
        mainAnimator.SetTrigger("Dying");
    }
}
