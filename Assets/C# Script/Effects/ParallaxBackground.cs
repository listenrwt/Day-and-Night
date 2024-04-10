using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public string bgName;  
    public Vector2 parallaxConst;
    private Transform mainCamtrans;
    [HideInInspector]
    public Vector3 startPos; 

    private void Awake()
    {
        mainCamtrans = Camera.main.transform;
        startPos = transform.position;
    }
    private void LateUpdate()
    {
        if (TimeManager.instance.ActionDisable)
            return;

        Vector3 dist = new Vector3(mainCamtrans.position.x * parallaxConst.x, mainCamtrans.position.y * parallaxConst.y, 0f);
        transform.position = new Vector3(startPos.x + dist.x, startPos.y + dist.y, transform.position.z);
        
    }

}
