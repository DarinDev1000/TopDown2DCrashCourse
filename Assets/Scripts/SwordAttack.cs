using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    public float damage = 1;
    public float timeBetweenAttacks = 0.5f;
    private float attackCooldown = 1f;
    private bool isCollidingWithEnemy = false;
    private List<Enemy> collidingEnemies;

    Vector2 attackOffsetX;

    // Start is called before the first frame update
    private void Start()
    {
        // swordCollider = GetComponent<Collider2D>();
        attackOffsetX = transform.localPosition;
        // Physics2D.IgnoreLayerCollision(0, 8);
        collidingEnemies = new();
    }

    private void Update()
    {
        attackCooldown += Time.deltaTime;
    }

    public void FaceRight()
    {
        transform.localPosition = attackOffsetX;
    }

    public void FaceLeft()
    {
        transform.localPosition = new Vector3(attackOffsetX.x * -1, attackOffsetX.y);
    }

    public void AttackRight()
    {
        // swordCollider.enabled = true;
        // transform.localPosition = attackOffsetX;
        DoAttack();
    }

    public void AttackLeft()
    {
        // swordCollider.enabled = true;
        // transform.localPosition = new Vector3(attackOffsetX.x * -1, attackOffsetX.y);
        DoAttack();
    }

    public void StopAttack()
    {
        // swordCollider.enabled = false;
    }

    //  // private void OnTriggerStay2D(Collider2D other) {
    //     private void OnTriggerEnter2D(Collider2D other) {
    //         print("OnTriggerStay2D");
    //         print(attackCooldown >= timeBetweenAttacks);
    //         print(other.tag);
    //         // If longer than attack cooldown, attack
    //         if (attackCooldown >= timeBetweenAttacks) {
    //             attackCooldown = 0f;
    //             if (other.tag == "Enemy") {
    //                 print(attackCooldown);
    //                 print(timeBetweenAttacks);
    //                 // Deal damage to enemy
    //                 Enemy enemy = other.GetComponent<Enemy>();

    //                 if (enemy != null) {
    //                     enemy.Health -= damage;
    //                 }
    //             }
    //         }
    //     }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // print("OnTriggerEnter2D");
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                collidingEnemies.Add(enemy);
                // print($"Touching {collidingEnemies.Count} enemies");
                isCollidingWithEnemy = true;
            }
        }
        // else
        // {
        //     Physics2D.IgnoreCollision(other, swordCollider);
        // }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // print("OnTriggerExit2D");
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                collidingEnemies.Remove(enemy);
                isCollidingWithEnemy = false;
            }
        }
    }

    // public void OnCollisionEnter(Collider2D collision) {
    //     print(collision.tag);
    //     Physics2D.IgnoreCollision(collision, swordCollider);
    // }

    private void DoAttack()
    {
        if (CheckAttackCooldown())
        {
            attackCooldown = 0f;
            // print(isCollidingWithEnemy);
            // print($"collidingEnemies {collidingEnemies.Count}");
            if (isCollidingWithEnemy && collidingEnemies.Count > 0)
            {
                List<Enemy> enemiesToRemove = new();
                foreach (var collidingEnemy in collidingEnemies)
                {
                    collidingEnemy.Health -= damage;
                    if (collidingEnemy.Health <= 0)
                    {
                        enemiesToRemove.Add(collidingEnemy);
                    }
                }
                foreach (var removeEnemy in enemiesToRemove)
                {
                    collidingEnemies.Remove(removeEnemy);
                }

            }
        }
    }

    public bool CheckAttackCooldown()
    {
        // print(attackCooldown);
        if (attackCooldown >= timeBetweenAttacks) return true;
        return false;
    }

    public void ResetAttackCooldown()
    {
        attackCooldown = 0f;
    }
}
