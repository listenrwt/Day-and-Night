using UnityEngine;
using System.Collections;

public class objectBehaviour : MonoBehaviour, IDataTransferable<objectBehaviour>     
{
    public objectList list;
    public string key;
    [HideInInspector]
    public int TimeIndex = 0;

    private bool changed = true;

    protected SpriteRenderer[] childSR;


    private void Start()
    {
        if(list != null)
        {
            for (int i = 0; i < list.objects.Length; i++)
            {
                if(list.objects[i].key == key)
                {
                    TimeIndex = i;
                    break;
                }
            }
        }
        childSR = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (list == null)
            return;
   
        if (TimeManager.instance.changingTime)
        {
            if(!changed)
            changeStatus();

        }else
        {
            changed = false;
        }
    }

    void changeStatus()
    {
        changed = true;

        int newTimeIndex = TimeIndex + (int)TimeManager.instance.time;
        if (newTimeIndex < 0 || newTimeIndex >= list.objects.Length)
            return;

        TurnToMask();
        InstantiateNewObject(newTimeIndex);       
    }

    void TurnToMask()
    {
        TurnParticleMask(gameObject, SpriteMaskInteraction.VisibleOutsideMask);

        if (childSR != null)
        {
            foreach (SpriteRenderer sr in childSR)
            {
                
                if (sr == null)              
                    continue;

                sr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
      
            }
        }

        DisableOldGlobalLight();
        
    }

    void InstantiateNewObject(int _newTimeIndex)
    {
        objectInfo newOBInfo = list.objects[_newTimeIndex];
        objectInfo oldOBInfo = list.objects[TimeIndex];

        Vector3 newPos = transform.position - oldOBInfo.PositionOffset + newOBInfo.PositionOffset;

        Quaternion newQua;
        if (newOBInfo.needRotationOffset)
        {
            newQua = transform.rotation
        * Quaternion.Euler(-oldOBInfo.RotationOffset)
        * Quaternion.Euler(newOBInfo.RotationOffset);
        }else
        {
            newQua = Quaternion.identity;
        }
      

        GameObject newObject = (GameObject)Instantiate(newOBInfo.Object, newPos, newQua);

        //Transfer data
        IDataTransferable<objectBehaviour> sender_ob = gameObject.GetComponentInChildren<IDataTransferable<objectBehaviour>>();
        IDataTransferable<objectBehaviour> reciever_ob = newObject.GetComponentInChildren<IDataTransferable<objectBehaviour>>();

        if (sender_ob != null && reciever_ob != null)
            reciever_ob.Recieve(sender_ob.Send());

        IDataTransferable<GameObject> sender = gameObject.GetComponentInChildren<IDataTransferable<GameObject>>();
        IDataTransferable<GameObject> reciever = newObject.GetComponentInChildren<IDataTransferable<GameObject>>();

        if(sender != null && reciever != null)
        reciever.Recieve(sender.Send());

         

        //For lighting
        LightFade(newObject);

        if (gameObject.transform.parent != null)
            newObject.transform.parent = gameObject.transform.parent;

        TurnToMask(newObject, SpriteMaskInteraction.VisibleInsideMask);

        StartCoroutine(DestroyObject(newObject));
    }

    void TurnToMask(GameObject _newObject, SpriteMaskInteraction SMI)
    {
        TurnParticleMask(_newObject, SMI);
        SpriteRenderer[] newchildSR = _newObject.GetComponentsInChildren<SpriteRenderer>();
        if(newchildSR != null)
        {
            foreach (SpriteRenderer sr in newchildSR)
            {
                if (sr == null)
                    continue;

                sr.maskInteraction = SMI;
            }
        }
        
    }

    void TurnParticleMask(GameObject ob, SpriteMaskInteraction SMI)
    {
        ParticleSystemRenderer[] PSRs = ob.GetComponentsInChildren<ParticleSystemRenderer>();
        foreach (ParticleSystemRenderer psr in PSRs)
        {
            psr.maskInteraction = SMI;
        }
    }

    IEnumerator DestroyObject(GameObject _newObject)
    {
        yield return new WaitUntil(() => changed == false);

        TurnToMask(_newObject, SpriteMaskInteraction.None);
        Destroy(gameObject);
    }

    protected virtual void LightFade(GameObject newObj)
    {
        //override by lightbehaviour
        return;
    }

    protected virtual void DisableOldGlobalLight()
    {
        //overide by lightbehaaviour
        //prevent global light error
        return;
    }

    public objectBehaviour Send()
    {
        return this;
    }

    public void Recieve(objectBehaviour data)
    {
        list = data.list;
    }
}
