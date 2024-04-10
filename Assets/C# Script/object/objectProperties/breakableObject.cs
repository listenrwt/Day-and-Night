using System.Collections;
using UnityEngine;

public class breakableObject : MonoBehaviour, IDataTransferable<GameObject>
{
    public GameObject breakparticle; 
    private objectBehaviour ob;
    public bool danger = true;

    private void Start()
    {
        ob = GetComponent<objectBehaviour>();
    }

    private bool started = true;
    private void Update()
    {
        if (!TimeManager.instance.ActionDisable)
        {
            started = false;
        }

        if(ob != null)
        {
            if(ob.TimeIndex == 0)
            {
                danger = true;
            }else
            {
                danger = false;
            }
            
        }

        if (TimeManager.instance.time == TimeManager.CurrentTime.Night && TimeManager.instance.ActionDisable && started == false)
        {
            StartCoroutine(WaitDestroyAtActionAble());
            started = true;
        }

    }

    IEnumerator WaitDestroyAtActionAble()
    {
        yield return new WaitUntil(() => !TimeManager.instance.ActionDisable);
        if (danger)
            DestroyStone();
    }


    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            int newTimeIndex = ob.TimeIndex - 1;
            if (newTimeIndex < 0)
            {
                DestroyStone();

            }else
            {
                Instantiate(ob.list.objects[newTimeIndex].Object, transform.position, transform.rotation);
                DestroyStone();
            }
               
        }
    }

    public void DestroyStone()
    {
        if (breakparticle != null)
            Instantiate(breakparticle, transform.position, Quaternion.identity);

        AudioManager.instance.PlayAudio("break");

        Destroy(gameObject);
    }

    public GameObject Send()
    {
        return gameObject;
    }

    public void Recieve(GameObject data)
    {
        objectBehaviour dataob = data.GetComponent<objectBehaviour>();
        if(dataob.TimeIndex + (int)TimeManager.instance.time < 0)
        {   
            DestroyStone();
        }
    }
}
