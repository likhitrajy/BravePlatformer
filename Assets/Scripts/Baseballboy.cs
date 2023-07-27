using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baseballboy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    

    private Rigidbody2D enemyBody;
    private Transform playerTransform;
    private Vector2 randomPosition;
    private bool hasReachedRandomPosition = true;

    public LayerMask groundLayer;
     private Animator enemyState;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GetRandomPosition();
        enemyState = GetComponent<Animator>();
    }

    void Update()
    {
        if (IsPlayerBetweenPoints())
        {
           
            float moveDirection = (playerTransform.position.x - transform.position.x);
            enemyBody.velocity = new Vector2(moveDirection,transform.position.y).normalized * moveSpeed * 2f;
            enemyState.SetBool("isRunning", true);

            
            if (moveDirection > 0f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (moveDirection < 0f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            
            GroundCheck();
        }
        else
        {
            if (hasReachedRandomPosition)
            {
             
                Vector2 moveDirection = (randomPosition - (Vector2)transform.position).normalized;
                enemyBody.velocity = moveDirection * moveSpeed;
                enemyState.SetBool("isRunning", true);

               
                if (moveDirection.x > 0f)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else if (moveDirection.x < 0f)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }

               
                if (Vector2.Distance(transform.position, randomPosition) < 0.5f)
                {
                    hasReachedRandomPosition = false;
                    StartCoroutine(WaitAndChooseRandomPosition());
                }

                GroundCheck();
            }
        }
    }

    bool IsPlayerBetweenPoints()
    {
      
        float playerX = playerTransform.position.x;
        float startX = startPoint.position.x;
        float endX = endPoint.position.x;

        return (playerX >= Mathf.Min(startX, endX) && playerX <= Mathf.Max(startX, endX));
    }

    IEnumerator WaitAndChooseRandomPosition()
    {
        enemyState.SetBool("isRunning", false);
       
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        GetRandomPosition();
        hasReachedRandomPosition = true;
        
    }

    void GetRandomPosition()
    {
      
        float randomX = Random.Range(startPoint.position.x, endPoint.position.x);
        float randomY = transform.position.y; 
        randomPosition = new Vector2(randomX, randomY);
    }

    void GroundCheck()
    {
  
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);

        if (hit.collider == null)
        {
           
            enemyBody.velocity = new Vector2(enemyBody.velocity.x, 0f);
        }
    }
}

