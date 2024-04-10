using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("PlayerStatus")]
    public float MoveSpeed;
    public float JumpForce;

    [Header("GroundCheck")]
    public LayerMask groundLayer;
    public LayerMask floorLayer; 
    public Transform groundCheck;
    public float groundcheckRadius;
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool isFloored = false;


    [HideInInspector]
    public Vector2 velocityRegister;

    [HideInInspector]
    public float x_move = 0f;
    [HideInInspector]
    public Rigidbody2D rb;
    private PlayerAnimator pa;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pa = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        if (TimeManager.instance.ActionDisable || SceneLoadingManager.isFading)
            return;

        x_move = Input.GetAxisRaw("Horizontal");

        //Check ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundcheckRadius, groundLayer);
        //Check floor
        isFloored = Physics2D.OverlapCircle(groundCheck.position, groundcheckRadius, floorLayer);

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && (isGrounded || isFloored))
        {
            pa.StartJump();
            Jump();           
        }

        //CheatMode
        if(Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.M) && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
            rb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (TimeManager.instance.ActionDisable)
            return;

        HorMove();
    }

    void HorMove()
    {
        //rb.velocity = new Vector2(0f, rb.velocity.y);
        float d = MoveSpeed * x_move;
        //transform.position += new Vector3(d, 0f, 0f);
        rb.velocity = new Vector2(d, rb.velocity.y);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        AudioManager.instance.PlayAudio("jump");
    }
    
    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundcheckRadius);
    }

}
