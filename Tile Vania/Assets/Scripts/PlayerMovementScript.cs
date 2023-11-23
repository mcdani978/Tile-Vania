using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] float Flt_runSpeed = 10f;
    [SerializeField] float Flt_jumpSpeed = 5f;
    [SerializeField] float Flt_ClimbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float Flt_gravityScaleAtStart;

    bool Bol_isAlive = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        Flt_gravityScaleAtStart = myRigidbody.gravityScale;
    }


    void Update()
    {
        if (!Bol_isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

   void OnFire(InputValue value)
    {
        if (!Bol_isAlive) 
        {
            return;
        }

        Instantiate(bullet, gun.position, transform.rotation);
    }

    void OnMove(InputValue value)
    {
        if (!Bol_isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue Value)
    {
        if (!Bol_isAlive) { return; }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if (Value.isPressed)
        {
            //do stuff
            myRigidbody.velocity += new Vector2(0f, Flt_jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * Flt_runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool Bol_playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", Bol_playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool Bol_playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (Bol_playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }



    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = Flt_gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * Flt_ClimbSpeed);

        myRigidbody.velocity = climbVelocity;

        myRigidbody.gravityScale = 0f;

        bool Bol_playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", Bol_playerHasVerticalSpeed);
    }

    void Die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            Bol_isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
        }

    }
}

        
    
    