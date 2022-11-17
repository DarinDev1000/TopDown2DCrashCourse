using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;

    private float health = 2;
    public float Health {
        set {
            // print($"Health: {value}");
            health = value;
            if (health <= 0) {
                Defeated();
            }
        }
        get {
            return health;
        }
    }

    public enum EnemyAnimationState {
        idle,
        lowJumping,
        highJumping,
        isWorried
    }
    // EnemyAnimationState animationState = EnemyAnimationState.idle;

    void Start() {
        animator = GetComponent<Animator>();
    }

    public void Defeated() {
        animator.SetTrigger("Defeated");
    }

    public void RemoveEnemy() {
        Destroy(gameObject);
    }

    private void FixedUpdate() {
        // Change animation if low health
        if (Health <= 1) {
            SetAnimationState(EnemyAnimationState.isWorried);
        } else {
            // Else Randomly update animation
            System.Random rnd = new();
            int number = rnd.Next(0,201);
            if (number == 0) {
                Idle();
            } else if (number == 1) {
                Jump();
            }
        }
    }

     public void Idle() {
        SetAnimationState(EnemyAnimationState.idle);
    }

    public void Jump() {
        System.Random rnd = new();
        int number = rnd.Next(0,2);
        if (number == 0) {
            SetAnimationState(EnemyAnimationState.lowJumping);
        } else {
            SetAnimationState(EnemyAnimationState.highJumping);
        }
    }

    public void SetAnimationState(EnemyAnimationState state) {
         animator.SetBool("lowJumping", false);
         animator.SetBool("highJumping", false);
         animator.SetBool("isWorried", false);

        //  Set animation state true
        if (state != EnemyAnimationState.idle) {
            animator.SetBool(state.ToString(), true);
        }
    }
}
