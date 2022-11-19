using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private GameObject target;

    public float speed;
    public float detectionDistance;
    private float distanceToTarget;
    public float randomVectorVariation = 1f;
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

    // private Collider2D enemyCollider;
    public float damage = 1;
    public float timeBetweenAttacks = 0.5f;

    private float attackCooldown = 100f;
    private bool isCollidingWithPlayer = false;
    private PlayerController collidingPlayer;

    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        // enemyCollider = GetComponent<Collider2D>();
        target = GameObject.FindGameObjectWithTag("Player");
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
        // Update attack cooldown timer
        attackCooldown += Time.deltaTime;

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

        // Try to attack player
        if (isCollidingWithPlayer)
        // print($"isCollidingWithPlayer {isCollidingWithPlayer}");
        {
            DoAttack();
        }

        // Find distance to target and start moving
        distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        if (distanceToTarget <= detectionDistance)
        {
            MoveToTarget();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // print("slime OnTriggerEnter2D");
        // print($"tag {other.tag}");
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerController>(out var player))
            {
                // print("colliding with player");
                // collidingEnemies.Add(player);
                collidingPlayer = player;
                isCollidingWithPlayer = true;
            }
        }
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     print("slime OnCollisionEnter2D");
    //     // print($"tag {collision}");
    // }

    private void OnTriggerExit2D(Collider2D other)
    {
        // print("slime OnTriggerExit2D");
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerController>(out var player))
            {
                // collidingEnemies.Remove(enemy);
                collidingPlayer = null;
                isCollidingWithPlayer = false;
            }
        }
    }

    public void DoAttack()
    {
        if (CheckAttackCooldown())
        {
            attackCooldown = 0f;
            // print($"attackCooldown {attackCooldown}");
            // print($"collidingEnemies {collidingEnemies.Count}");
            if (isCollidingWithPlayer && collidingPlayer != null)
            {
                // List<Enemy> enemiesToRemove = new();
                // foreach (var collidingEnemy in collidingEnemies)
                // {
                // print($"collidingPlayer health {collidingPlayer.Health}");
                // print($"damage {damage}");
                collidingPlayer.TakeDamage(damage);
                // print($"collidingPlayer health {collidingPlayer.Health}");
                if (collidingPlayer.Health <= 0)
                {
                    // enemiesToRemove.Add(collidingEnemy);
                    collidingPlayer = null;
                }
            }
            // foreach (var removeEnemy in enemiesToRemove)
            // {
            //     collidingEnemies.Remove(removeEnemy);
            // }

        }
    }

    public bool CheckAttackCooldown()
    {
        if (attackCooldown >= timeBetweenAttacks) return true;
        return false;
    }

    public void ResetAttackCooldown()
    {
        attackCooldown = 0f;
    }

    private void MoveToTarget()
    {
        float moveSpeed = speed * Time.deltaTime;

        float random1 = Random.Range(-randomVectorVariation, randomVectorVariation);
        float random2 = Random.Range(-randomVectorVariation, randomVectorVariation);
        // Vector2 currentVariation = new Vector2(transform.position.x + Random.Range(-randomVectorVariation, randomVectorVariation), transform.position.y + Random.Range(-randomVectorVariation, randomVectorVariation));
        Vector2 targetVariation = new Vector2(target.transform.position.x + random1, target.transform.position.y + random2);

        transform.position = Vector2.MoveTowards(transform.position, targetVariation, moveSpeed);
    }
}
