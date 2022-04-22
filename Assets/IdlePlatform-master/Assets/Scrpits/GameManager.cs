using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 2;
    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2f;
    private float x;
    public float jumpForce;
    private bool isGrounded = false;
    public Transform isGroundedChecker;
    private float checkGroundRadius = 0.05f;
    public LayerMask groundLayer;
    private float rememberGroundedFor = 0.3f;
    public float lastTimeGrounded;
    private Animator animator;
    public int coin = 0;
    public Text coinText; 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        coinText.text = coin.ToString(); 
        Move();
        Jump();
        CheckIfGrounded();
        BetterJump();
        if ( x==0)
        {
            animator.SetBool("isWalking", false); 
        }
        else 
        {
            animator.SetBool("isWalking", true);
        }
    }
    void Move()
    {
        //x = Input.GetAxis("Horizontal");

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Debug.Log("1 ");
        //}


        if (Input.GetKey(KeyCode.D))
        {
            x = 1;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            x = -1;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            

        }
        else
        {
            x = 0;
        }
        float moveBy = x * speed;
        rb.velocity = new Vector2(moveBy, rb.velocity.y);


    }
    void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor))
        {
            animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }


        //if (Input.GetKeyDown(KeyCode.Space)&&isGrounded)
        //{
        //   rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //  //  rb.AddForce(transform.up*jumpForce*50);
        //}
    }


    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        //if (colliders != null)
        //{
        //    isGrounded = true;
        //}
        //else
        //{
        //    if (isGrounded)
        //    {
        //        lastTimeGrounded = Time.time;
        //    }
        //    isGrounded = false;
        //}
        if (collider != null)
        {
            isGrounded = true;
            animator.SetBool("isJumpLoop", false);
        }
        else
        {
            isGrounded = false;
        }
    }
    void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void JumpLoop ()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isJumpLoop", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            coin++;
            Destroy(collision.gameObject);
        }

    }
}
