using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D dogBody;
    public GameObject startPoint;
    public GameObject endPoint;
    private Animator dogState;
    private Transform currentPoint;
    


    void Start()
    {
        dogBody = GetComponent<Rigidbody2D>();
        dogState = GetComponent<Animator>();
        currentPoint = endPoint.transform;
        dogState.SetBool("isRunning", true);

     
    }

    void Update()
    {
          FollowPath();
        
    }

    void FollowPath(){
        if(currentPoint == endPoint.transform){
            dogBody.velocity = new Vector2(moveSpeed, 0);
        }
        else{
             dogBody.velocity = new Vector2(-moveSpeed, 0);
            
        }
        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == endPoint.transform){
            FlipEnemyFacing();
            currentPoint = startPoint.transform;
        }
        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == startPoint.transform){
            FlipEnemyFacing();
            currentPoint = endPoint.transform;
        }
    }

   
    void FlipEnemyFacing(){
         Vector3 localScale = transform.localScale;
         localScale.x *= -1;
         transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player" ){
            dogState.SetTrigger("Attacking");
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        dogState.ResetTrigger("Attacking");
    }
    
    

        
}
