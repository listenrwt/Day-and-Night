using UnityEngine;

public class BackGroundManager : MonoBehaviour,IDataTransferable<GameObject>
{
    [HideInInspector]
    public ParallaxBackground[] semiBG; 

    public void Recieve(GameObject data)
    {
        semiBG = gameObject.GetComponentsInChildren<ParallaxBackground>();

        BackGroundManager dataBGM = data.GetComponent<BackGroundManager>();

        foreach (ParallaxBackground data_semiBG in dataBGM.semiBG)
        {
            foreach (ParallaxBackground _semiBG in semiBG)
            {
                if (data_semiBG.bgName == _semiBG.bgName)
                {
                    _semiBG.transform.position = data_semiBG.transform.position;
                    _semiBG.startPos = data_semiBG.startPos;
                }
            }
        }
    }

    public GameObject Send()
    {
        return gameObject;
    }

    private void Start()
    {
        semiBG = gameObject.GetComponentsInChildren<ParallaxBackground>();
    }

}
