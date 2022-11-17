using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{
    Vector2 movementInput;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        
    }

    // private void FixedUpdate() {
    //     // If movement input is not 0, try to move
    //     if (movementInput != Vector2.zero){
            
    //     }
    // }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }
}
