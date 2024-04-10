using UnityEngine;
using System.Collections;


public class PlantsBehaviour : MonoBehaviour
{
    [Header("IdleProp")]
    public PlantProp AnticlockwiseIdle;
    public PlantProp ClockwiseIdle;
    public bool clockwiseTurn = false;

    private float offsetAngle;    
    private float idleTimer = 0f;
    
    private bool isIdle = true;



    private void Start()
    {
        offsetAngle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
    }

    
    void PlayIdle(PlantProp prop)
    {
        if(prop.direction == PlantProp.Direction.None)
        {
            Debug.LogError("Non-directional is not allowed in idle move.");
            return;
        }

        idleTimer += Time.deltaTime;

        float MaxAngle = prop.RotaAngle * Mathf.Deg2Rad;
        float half_period = prop.RotaTime;
        float x = idleTimer;
        float x_shift = prop.PeriodShift;
        float y_shift = offsetAngle;

        float z_rotation = (MaxAngle * Mathf.Sin(Mathf.PI / half_period * x + x_shift) + y_shift) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, z_rotation);

        if (idleTimer >= prop.RotaTime)
        {
            idleTimer = 0f;
            clockwiseTurn = !clockwiseTurn;
        }
            
    }

    private PlantProp interactProp = null;
    private float originalAngle = 0f;
    private float interactTimer = 0f;
    private bool resetToIdle = false;
    void PlayInteraction(PlantProp prop)
    {

        interactTimer += Time.deltaTime;

        float MaxAngle = prop.RotaAngle * Mathf.Deg2Rad;
        float half_period = prop.RotaTime;
        float x = interactTimer;
        float x_shift = prop.PeriodShift;
        float y_shift = originalAngle;

        float z_rotation = (MaxAngle * Mathf.Sin(Mathf.PI / half_period * x + x_shift) + y_shift) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, z_rotation);

        if(interactTimer >= prop.RotaTime / 2)
        {
            interactTimer = 0f;

            if (!resetToIdle)
            {
                resetToIdle = true;

                float rotaAngle = offsetAngle * Mathf.Rad2Deg - transform.rotation.eulerAngles.z;
                if(rotaAngle > 180)
                {
                    rotaAngle -= 360;
                }else 
                if(rotaAngle < -180)
                {
                    rotaAngle += 360;
                }

                if(rotaAngle >= 0)
                {
                    clockwiseTurn = false;  
                }else
                {
                    clockwiseTurn = true;
                }

                float rotaTime = Mathf.Abs(rotaAngle) * prop.RotaTime / prop.RotaAngle * 2f;
                interactProp = new PlantProp(rotaAngle, rotaTime, PlantProp.Direction.None);
                
                originalAngle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

            }else
            {
                resetToIdle = false;
                idleTimer = 0f;                             
                interactProp = null;
                isIdle = true;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interact = collision.GetComponent<IInteractable>();        
        if(isIdle && interact != null)
        {
            PlantProp prop = interact.getPlantProp();
            if(prop.direction != PlantProp.Direction.None)
            {
                isIdle = false;
                originalAngle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
                interactProp = prop;
            }
        }
    }

    private void Update()
    {
        if(TimeManager.instance != null)
        if (TimeManager.instance.ActionDisable)
            return;

        if (isIdle)
        {
            if (clockwiseTurn)
            {
                PlayIdle(ClockwiseIdle);
            }
            else
            {
                PlayIdle(AnticlockwiseIdle);
            }

        }else
        {
            if(interactProp != null)
            PlayInteraction(interactProp);
        }

    }

}

[System.Serializable]
public class PlantProp
{
    public float RotaAngle = 0f;
    public float RotaTime = 0f;
    public enum Direction {None ,clockwise, anticlockwise };
    public Direction direction = Direction.None;

    public PlantProp() { }

    public PlantProp(float rotaAngle, float rotaTime, float velocityX)
    {
        RotaAngle = rotaAngle;
        RotaTime = rotaTime;
        if(velocityX > 0f)
        {
            direction = Direction.clockwise;

        }else if(velocityX < 0f)
        {
            direction = Direction.anticlockwise;
        }
        else
        {
            direction = Direction.None;
        }
    }

    public PlantProp(float rotaAngle, float rotaTime, Direction dir)
    {
        RotaAngle = rotaAngle;
        RotaTime = rotaTime;
        direction = dir;
    }

    public float PeriodShift
    {
        get
        {
            if (direction == Direction.clockwise)
            {
                return Mathf.PI;
            }
            else if (direction == Direction.anticlockwise)
            {
                return 0f;
            }
            else
            {
                return 0f;
            }
        }    
    }

}








