using UnityEngine;

public class PlayerAnimator : MonoBehaviour, IInteractable  
{
    [Header("GroundCheckAnimation")]
    public LayerMask groundLayer;
    public LayerMask floorLayer;
    public Transform groundCheck;
    public float groundcheckRadius;   
    private bool checkGround;

    private Animator ani; 
    private PlayerMovement pm;
    private Rigidbody2D playerRB;

    [Header("PlantsInteraction")]
    public float plantsRotaAngle = 0f;
    public float plantsRotaTime = 0f;

    [Header("PassAnimation")]
    public float attractSpeed;
    public float rotaSpeed;
    public float reduceSpeed;
    [HideInInspector]
    public Transform endgate;


    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        ani = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        ani.SetBool("Passed", false);
    }

    private void Update()
    {
        if(TimeManager.instance != null)
        ani.enabled = !TimeManager.instance.ActionDisable;

        if (!EndGate.passed || (transform.localScale.y <= 0f && transform.localScale.x <= 0f))
        {
            #region Flipping
            float Vx = pm.x_move;

            if (Vx > 0)
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            else if (Vx < 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            }
            #endregion
            //Get Var
            ani.SetInteger("x_move", (int)Vx); //Running
            ani.SetFloat("VelocityY", playerRB.velocity.y); // check velocity of y-axis

            checkGround = Physics2D.OverlapCircle(groundCheck.position, groundcheckRadius, groundLayer);
            if (!checkGround)
                checkGround = Physics2D.OverlapCircle(groundCheck.position, groundcheckRadius, floorLayer);

            ani.SetBool("checkGround", checkGround);

            if (checkGround && playerRB.velocity.y < -1f)
            {
                AudioManager.instance.PlayCheckAudio("landing");
            }

        }else
        {
            //PassAnimation
            transform.position = Vector2.MoveTowards(transform.position, endgate.position, attractSpeed * Time.deltaTime);
            transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, rotaSpeed));
            ani.SetBool("Passed", true);
        }

    }

    public void StartJump()
    {
        ani.SetTrigger("TakingOff"); // TakeOffAnimation
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundcheckRadius);
    }

    public PlantProp getPlantProp()
    {
        return new PlantProp(plantsRotaAngle, plantsRotaTime, playerRB.velocity.x);
    }
}
