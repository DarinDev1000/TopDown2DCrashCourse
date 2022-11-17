using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    public float damage = 0.5f;

    Vector2 attackOffsetX;

     // Start is called before the first frame update
    private void Start() {
        attackOffsetX = transform.localPosition;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy") {
            // Deal damage to enemy
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null) {
                enemy.Health -= damage;
            }
        }
    }
}
