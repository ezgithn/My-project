using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //private PlayerController Instance;
    public ParticleSystem Dust;
    private Rigidbody2D rb;
    private Animator animator;

    //Jump
    public float jumpForce;
    public float jumpTime;
    public float fallMultiplier;
    public float jumpMultiplier;
    private float extraJump;
    public float extraJumpValue;
    public float jumpHeight;

    //Attack
    public GameObject attackCollider;
    public float attackDuration;
    public float attackCooldown;
    private bool isAttacking;
    private float attackTime;
    private float cooldownTime;
    public KeyCode attackKey;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayers;


    public float speed;
    private readonly float idle;
    public float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    private float slowRunSpeed;
    private bool facingRight = true;

    //Dash
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    private bool isDashing;
    private float dashTimeLeft;
    private float dashCooldownLeft;
    public KeyCode dashKey;

    private Vector2 moveInput;
    public float horizontalMove;
    private float verticalMove;
    public bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private Vector2 vgravity;

    public TrailRenderer tRenderer;
   



    #region Unity Methods

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackCollider = GetComponent<GameObject>();

        extraJump = extraJumpValue;

        isDashing = false;
        dashTimeLeft = 0;
        dashCooldownLeft = 0;

        isAttacking = true;
        attackTime = Time.deltaTime;
        cooldownTime = Time.deltaTime;

    }

    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.01f, 0.3f), CapsuleDirection2D.Horizontal, 0.1f, groundLayer);
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Walk", Mathf.Abs(horizontalMove) > 0);
        animator.SetBool("isAttacking", true && Input.GetKey(attackKey));

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("Run", true);
                animator.SetBool("Walk", false);

                moveSpeed = runSpeed;
            }
            else
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Run", false);

                moveSpeed = walkSpeed;
            }

            Vector3 moveDirection = Vector3.zero;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveDirection += Vector3.left;
                if(isGrounded)
                {
                    CreateDust();
                }
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveDirection += Vector3.right;
                if (isGrounded)
                {
                    CreateDust();
                }
            }

            Vector3 moveInput = new (horizontalMove, 0f, 0f);
            transform.position += moveSpeed * Time.deltaTime * moveDirection;

        }
       

        if (horizontalMove > 0 && facingRight)
        {
            gameObject.transform.localScale = new Vector3(10, 10, 10);
        }

        if(horizontalMove < 0 && facingRight)
        {
            gameObject.transform.localScale = new Vector3(-10, 10, 10);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && extraJump > 0)
        {
            rb.velocity = Vector2.up * jumpForce * jumpMultiplier;
            animator.SetBool("Jump", true);
            extraJump--;
        }
        if (isGrounded && animator.GetBool("Jump") && extraJump == extraJumpValue)
        {
            animator.SetBool("Jump", false);
            
        }
        else if (extraJump == 0 && isGrounded == true)
        {
            //rb.velocity = Vector2.up * jumpForce * jumpMultiplier;
            //animator.SetBool("Jump", false);
            extraJump = extraJumpValue;
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += (fallMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
            if (isGrounded)
            {
                CreateDust();
            }
        }

        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKey(dashKey) && dashCooldownLeft <= 0)
        {
            isDashing = true;
            dashTimeLeft = dashDuration;
            dashCooldownLeft = dashCooldown;
            tRenderer.emitting = true;
        }

        if (isDashing)
        {
            rb.velocity = moveInput.normalized * dashSpeed;
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                rb.velocity = Vector2.zero;
                tRenderer.emitting = false;
            }
        }

        if (dashCooldownLeft > 0)
        {
            dashCooldownLeft -= Time.deltaTime;
        }

        if (Input.GetKey(attackKey) && !isAttacking && Time.deltaTime > cooldownTime)
        {
            isAttacking = true;
            attackTime = Time.deltaTime;
            cooldownTime = Time.deltaTime + attackCooldown;
            attackCollider.SetActive(true);
            Invoke(nameof(DeactivateAttackCollider), Time.deltaTime * 200);
        }


        GetComponent<Animator>().SetFloat("Speed", moveInput.magnitude);
        
    }
    #endregion

    void Attack()
    {
        animator.SetBool("isAttacking", true);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit" + enemy.name);
        }
    } 

    private void DeactivateAttackCollider()
    {
        isAttacking = false;
        attackCollider.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

  
    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -10;
        gameObject.transform.localScale = currentScale;
        facingRight = !facingRight;
    }

    private void CreateDust()
    {
        Dust.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
    }
  
}
