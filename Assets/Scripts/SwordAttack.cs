using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    public float damage = 1;
    public float timeBetweenAttacks = 0.5f;
    private float attackCooldown = 0f;

    Vector2 attackOffsetX;

     // Start is called before the first frame update
    private void Start() {
        attackOffsetX = transform.localPosition;
    }

        private void Update() {
        attackCooldown += Time.deltaTime;
    }

    public void AttackRight() {
        swordCollider.enabled = true;
        transform.localPosition = attackOffsetX;
    }

    public void AttackLeft() {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(attackOffsetX.x * -1, attackOffsetX.y);
    }
    public void StopAttack() {
        swordCollider.enabled = false;
    }

    // private void OnTriggerStay2D(Collider2D other) {
    private void OnTriggerEnter2D(Collider2D other) {
        print("OnTriggerStay2D");
        print(attackCooldown >= timeBetweenAttacks);
        print(other.tag);
        // If longer than attack cooldown, attack
        if (attackCooldown >= timeBetweenAttacks) {
            attackCooldown = 0f;
            if (other.tag == "Enemy") {
                print(attackCooldown);
                print(timeBetweenAttacks);
                // Deal damage to enemy
                Enemy enemy = other.GetComponent<Enemy>();

                if (enemy != null) {
                    enemy.Health -= damage;
                }
            }
        }
    }

    public bool CheckAttackCooldown() {
        print(attackCooldown);
        if (attackCooldown >= timeBetweenAttacks) return true;
        return false;
    }

     public void ResetAttackCooldown() {
       attackCooldown = 0f;
    }
}
