using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour 
{
    public float JumpForce = 700f;
    public float Speed = 1f;
    public float MaxSpeed = 10f;
    public float groundRadius = 0.2f;
    public float wallRadius = 0.3f;
  
    bool grounded = true;
    bool facingRight = true;
    bool onWall = false;
    bool doubleJump = false;

    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;

    Animator anim;
    CircleCollider2D cc;
    BoxCollider2D bc;

	void Start () 
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CircleCollider2D>();
        bc = GetComponent<BoxCollider2D>();
	}
	void FixedUpdate () 
    {
        float move = Input.GetAxis("Horizontal");

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        onWall = Physics2D.OverlapCircle(wallCheck.position, wallRadius, whatIsWall);
        rigidbody2D.velocity = new Vector2(move * MaxSpeed, rigidbody2D.velocity.y);
        anim.SetBool("Ground", grounded);
        anim.SetFloat("Speed", Mathf.Abs(move));

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
	}
    void Update()
    {
        if(grounded || onWall)
            doubleJump = true;

        Debug.Log(doubleJump);
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
                StopCoroutine("DisableCollision");
                StartCoroutine("DisableCollision");
                rigidbody2D.AddForce(new Vector2(0, -JumpForce));
                StartCoroutine("ReEnableCollision");
        }

        
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (grounded)
            {
                anim.SetBool("Ground", false);
                rigidbody2D.AddForce(new Vector2(0, JumpForce));
                doubleJump = true;
            }
            else if(!grounded && doubleJump)
            {
                Debug.Log("AnimSet");
                anim.SetBool("Ground", false);
                rigidbody2D.AddForce(new Vector2(0, JumpForce));

                doubleJump = false;
            }
        }
        if(onWall)
        {
            anim.SetBool("OnWall", true);
        }
    }
    void OnGUI()
    {
        // Debug
        //GUI.Label(new Rect(0, 0, 100, 100), "On Wall: " + onWall.ToString());
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    IEnumerator DisableCollision()
    {
        cc.enabled = false;
        bc.enabled = false;
        yield return new WaitForSeconds(1.5f);
    }
    IEnumerator ReEnableCollision()
    {       
        cc.enabled = true;
        bc.enabled = true;
        yield return null;
    }
}
