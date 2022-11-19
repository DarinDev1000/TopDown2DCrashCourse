using System.Collections.ObjectModel;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int speed = 2;
    private Vector2 movement;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        // Variant 1
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement);

        // Variant 2
        // Should be use with linear friction
        // if (movement.x != 0 || movement.y != 0)
        // {
        //     rb.velocity = movement * speed;
        // }

        // Variant 3
        // rb.AddForce(movement * speed);
    }
}
