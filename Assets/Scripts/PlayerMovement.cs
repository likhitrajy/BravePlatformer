using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerBody; 
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 1f;
    Animator playerState;
    CapsuleCollider2D playerCollider;
    BoxCollider2D playerFeetCollider;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerState = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
    }

  
    void Update()
    {
        Run();
        FlipSprite();
    }

    void OnMove(InputValue value){
        moveInput = value.Get<Vector2>();
    }

     void OnJump(InputValue value){
        if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {return;}
        if(value.isPressed){
            playerBody.velocity += new Vector2 (0f, jumpSpeed);
            playerState.SetTrigger("Jumping");
        }
        
       
    }

    void Run(){
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerBody.velocity.y);
        playerBody.velocity = playerVelocity;  
        bool playerHasHorizontalSpeed = Mathf.Abs(playerBody.velocity.x) > Mathf.Epsilon;
        playerState.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite(){
        bool playerHasHorizontalSpeed = Mathf.Abs(playerBody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed){
        transform.localScale = new Vector2(Mathf.Sign(playerBody.velocity.x), 1f);
    }
    }
}
