using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    [HideInInspector]
    public GameObject pickedObject = null;

    [Header("checkPickableObjectCircle")]
    public Transform checkPickableObj;
    public float pickRadius;

    [Header("PickableObjectInfo")]
    public Transform SpawnPos;

    private PlayerMovement pm;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (TimeManager.instance.ActionDisable)
            return;

        if (Input.GetKeyDown(KeyCode.Q) && pm.isGrounded && pm.rb.velocity.y == 0f)
        {
            if(pickedObject == null)
            {
                DetectPickableObject();

                if (pickedObject != null)
                    PickUpObject();

            }else
            {               
                PutDownObject();
            }
        }
    }

    void DetectPickableObject()
    {
        GameObject[] pickableObjs = GameObject.FindGameObjectsWithTag("PickableObject");

        GameObject objToPick = null;
        float sqrMinDistance = Mathf.Infinity;
        foreach(GameObject obj in pickableObjs)
        {
            foreach (Transform componentTrans in obj.GetComponentsInChildren<Transform>())
            {
                Vector2 different = componentTrans.transform.position - checkPickableObj.position;
                float sqrDistance = different.sqrMagnitude;
                if (sqrDistance < pickRadius * pickRadius && sqrDistance < sqrMinDistance)
                {
                    objToPick = obj;
                    sqrMinDistance = sqrDistance;
                }
            }
            
        }           
        
        pickedObject = objToPick;       
    }

    private Vector3 pickedObjmovePos = Vector3.zero;

    void PickUpObject()
    {
        ChangeComponentStatus(false);

        PickableObject pobj = pickedObject.GetComponent<PickableObject>();
        pickedObjmovePos = SpawnPos.transform.position + (Vector3)pobj.GetPickedOffset() - pickedObject.transform.position;
        pickedObject.transform.position += pickedObjmovePos;

        AudioManager.instance.PlayAudio("pick");

    }

    void PutDownObject()
    {
        pickedObject.transform.position -= new Vector3(0f, pickedObjmovePos.y, 0f);
        pickedObjmovePos = Vector3.zero;

        ChangeComponentStatus(true);
        pickedObject = null;

        AudioManager.instance.PlayAudio("pick");
    }

    void ChangeComponentStatus(bool status)
    {
        objectBehaviour pickedObjBH = pickedObject.GetComponent<objectBehaviour>();
        pickedObjBH.enabled = status;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkPickableObj.position, pickRadius);
    }

}
