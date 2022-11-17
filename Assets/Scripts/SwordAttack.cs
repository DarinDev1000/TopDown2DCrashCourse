using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    // public enum AttackDirection {
    //     left,
    //     right
    // }
    // public AttackDirection attackDirection;

    Vector2 attackOffsetX;
    Collider2D swordCollider;

     // Start is called before the first frame update
    private void Start() {
        swordCollider = GetComponent<Collider2D>();
        attackOffsetX = transform.position;
    }

    // public void Attack() {
    //     switch (attackDirection)
    //     {
    //         case AttackDirection.left:
    //             AttackLeft();
    //             break;
    //         case AttackDirection.right:
    //             AttackRight();
    //             break;
    //     }
    // }

    public void AttackRight() {
        swordCollider.enabled = true;
        transform.position = attackOffsetX;
    }

    public void AttackLeft() {
        swordCollider.enabled = true;
        transform.position = new Vector3(attackOffsetX.x * -1, attackOffsetX.y);
    }
    public void StopAttack() {
        swordCollider.enabled = false;
    }
}
