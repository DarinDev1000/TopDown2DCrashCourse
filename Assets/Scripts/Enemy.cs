using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;

    public float maxHealth;
    private float health;
    public float Health
    {
        set
        {
            // print($"Health: {value}");
            health = value;
            if (health <= 0)
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }

    public enum EnemyAnimationState
    {
        isIdle,
        isLowJumping,
        isHighJumping,
        isWorried,
        isDefeated
    }
    // EnemyAnimationState animationState = EnemyAnimationState.idle;

    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void Defeated()
    {
        animator.SetBool(EnemyAnimationState.isDefeated.ToString(), true);
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        // Change animation if low health
        if (Health < maxHealth)
        {
            SetAnimationState(EnemyAnimationState.isWorried);
        }
        else
        {
            // Else Randomly update animation
            System.Random rnd = new();
            int number = rnd.Next(0, 201);
            if (number == 0)
            {
                Idle();
            }
            else if (number == 1)
            {
                Jump();
            }
        }
    }

    public void Idle()
    {
        SetAnimationState(EnemyAnimationState.isIdle);
    }

    public void Jump()
    {
        System.Random rnd = new();
        int number = rnd.Next(0, 2);
        if (number == 0)
        {
            SetAnimationState(EnemyAnimationState.isLowJumping);
        }
        else
        {
            SetAnimationState(EnemyAnimationState.isHighJumping);
        }
    }

    public void SetAnimationState(EnemyAnimationState state)
    {
        animator.SetBool("isLowJumping", false);
        animator.SetBool("isHighJumping", false);
        animator.SetBool("isWorried", false);

        //  Set animation state true
        if (state != EnemyAnimationState.isIdle)
        {
            animator.SetBool(state.ToString(), true);
        }
    }
}
