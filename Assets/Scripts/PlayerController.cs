using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    readonly List<RaycastHit2D> castCollisions = new();

    bool canMove = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPlayerMovement(Vector2 playerMovement)
    {
        movementInput = playerMovement;
    }

    private void FixedUpdate()
    {
        // If movement is locked, exit
        if (!canMove)
        {
            return;
        }

        // If movement input is not 0, try to move
        if (movementInput != Vector2.zero)
        {
            // Debug.Log($"X: {movementInput.x}  Y: {movementInput.y}");
            // print($"X: {movementInput.x}  Y: {movementInput.y}");
            bool success = TryMove(movementInput);

            // If we can't move in a diagonal direction, try moving in only X or Y
            if (!success)
            {
                // Try to move along X
                success = TryMove(new Vector2(movementInput.x, 0));
            }
            if (!success)
            {
                // Else try to move along Y
                success = TryMove(new Vector2(0, movementInput.y));
            }

            // Set isMoving if moving
            animator.SetBool("isMoving", success);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // Set direction of sprite to movement direction
        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
            swordAttack.FaceLeft();
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
            swordAttack.FaceRight();
        }
    }

    private bool TryMove(Vector2 direction)
    {
        // Check if no movement
        if (direction == Vector2.zero)
        {
            return false;
        }

        // Check for potential collisions
        int count = rb.Cast(
            direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
            movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
            castCollisions, // List of collisions to store the found collisions into after the Cast is finished
            moveSpeed * Time.fixedDeltaTime + collisionOffset // The amount to case equal to the movement plus an offset
        );

        // Move if no collisions
        if (count == 0)
        {
            rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
            return true;
        }
        return false;
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    // Attack on fire(left mouse) click
    void OnFire()
    {
        if (swordAttack.CheckAttackCooldown())
        {
            animator.SetTrigger("swordAttack");
        }
    }

    public void StartSwordAttack()
    {
        LockMovement();

        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
}
