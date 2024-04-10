using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FlysBehaviour : MonoBehaviour, IDataTransferable<GameObject>, IInteractable
{

    [Header("Motion")]
    public RangeValues rotationspeed = new RangeValues(0, 0, 0, 0);
    private float curRotaSpeed = 0f;
    public RangeValues time = new RangeValues(0, 0);
    private float timer = 0f;
    public float speed;
    public float fixRouteConst;
    
    [HideInInspector]
    public Vector3 trackpt;
    [Header("Track")]
    public float maxDistance;
    public float minDistance;
    private bool backed = true;
    private float oriAngle = 0f;
    private bool recordOnce = false;

    [Header("Interaction")]
    public float plantsRotaAngle;
    public float plantsRotaTime;

    private void Awake()
    {
        trackpt = transform.position;
    }
    private void Start()
    {
        time.current = time.NumGenerate();
        rotationspeed.current = rotationspeed.NumGenerate();
    }

    private void Update()
    {
        if(TimeManager.instance != null)
        if (TimeManager.instance.ActionDisable)
            return;

        transform.position += new Vector3(0f, speed * Time.deltaTime, 0f).magnitude * transform.up.normalized;

        if (Vector2.Distance(trackpt, transform.position) >= maxDistance || !backed)
        {

            Vector3 a = transform.up;
            Vector3 b = trackpt - transform.position;
            float angle = Mathf.Acos((a.x * b.x + a.y * b.y) / (a.magnitude * b.magnitude)); //Find the inc. angle between Vector a & b

            if(!recordOnce)
            {
                oriAngle = angle;
                recordOnce = true;
            }

            if(angle > 0.05f)
            {
                float distance = Vector2.Distance(transform.position, trackpt) - minDistance;
                if (distance < 1) distance = 1f;
                transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, angle * distance * distance / oriAngle  * fixRouteConst * Time.deltaTime));
            }
                
            backed = false;
            if (Vector2.Distance(trackpt, transform.position) <= minDistance)
            {
                backed = true;
                recordOnce = false;
            }
        }
        else
        {
            if (timer <= time.current)
            {
                curRotaSpeed += (rotationspeed.current - rotationspeed.previous) / time.current;
                if (curRotaSpeed > 300f || curRotaSpeed < -300f)
                    curRotaSpeed = -curRotaSpeed;
                transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, curRotaSpeed * Time.deltaTime));
                timer += Time.deltaTime;
            }
            else
            {
                rotationspeed.previous = rotationspeed.current;
                rotationspeed.current = rotationspeed.NumGenerate();
                time.current = time.NumGenerate();
                timer = 0f;
            }
        }     
    }
    public GameObject Send()
    {
        return gameObject;
    }

    public void Recieve(GameObject data)
    {
        FlysBehaviour fb = data.GetComponent<FlysBehaviour>();
        trackpt = fb.trackpt;
        curRotaSpeed = fb.curRotaSpeed;

        transform.rotation = data.transform.rotation;
    }

    public PlantProp getPlantProp()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        return new PlantProp(plantsRotaAngle, plantsRotaTime, -transform.rotation.z);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            trackpt = transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(trackpt, maxDistance);
            Gizmos.DrawWireSphere(trackpt, minDistance);
        }else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(trackpt, maxDistance);
            Gizmos.DrawWireSphere(trackpt, minDistance);
        }
        
    }
}

[System.Serializable]
public class RangeValues
{
    [HideInInspector]
    public float current = 0f;
    [HideInInspector]
    public float previous = 0f;
    public float min1;
    public float max1;
    public float min2;
    public float max2;
    private bool temp = true;

    public RangeValues(float _min, float _max)
    {
        min1 = _min;
        max1 = _max;
    }

    public RangeValues(float _min1, float _max1, float _min2, float _max2)
    {
        min1 = _min1;
        max1 = _max1;
        min2 = _min2;
        max2 = _max2;
    }

    public float NumGenerate()
    {
        if(max2 == 0 && min2 == 0)
        {
            return Random.Range(min1, max1); 
        }else
        {
            if (temp)
            {
                temp = false;
                return Random.Range(min1, max1);

            }else
            {
                temp = true;
                return Random.Range(min2, max2);
            }
        }
    }

}
