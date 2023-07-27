using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerBody; 
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public LayerMask enemyLayer;
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

    void OnJump(InputValue value)
    {
    bool isGrounded = playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

   
    if (isGrounded && value.isPressed)
    {
        playerBody.AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);
        playerState.SetBool("isJumping", true);
    }
    }

    void OnPunch(InputValue value){
        playerState.SetTrigger("Punching");
        Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies){
            enemy.GetComponent<Health>().TakeDamage(attackDamage);
        } 
            
        }
    

    void OnCollisionEnter2D(Collision2D collision)
    {

    if (collision.gameObject.CompareTag("Ground"))
    {
        playerState.SetBool("isJumping", false);
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

     private void OnDrawGizmosSelected() {
        if(attackPoint == null){ return;}
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
