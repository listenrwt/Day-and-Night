using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Rendering.Universal;

public class TailBehaviour : MonoBehaviour
{
    [Header("General")]
    public float duration;
    private SpriteRenderer SR;
    private Light2D light2D;

    [Header("Transparency")]
    public bool hasTransparent = false;
    public AnimationCurve Tcurve;

    [Header("LightIntensity")]
    public bool hasLight = false;
    public AnimationCurve Lcurve;

    private void Start()
    {
        if(hasTransparent)
            SR = gameObject.GetComponent<SpriteRenderer>();

        if(hasLight)
            light2D = gameObject.GetComponent<Light2D>();

        StartCoroutine(Deformation());
    } 

    IEnumerator Deformation()
    {
        float Timer = duration;

        while (Timer >= 0)
        {
            if (hasTransparent)
            {
                float alpha = Tcurve.Evaluate(Timer / duration);
                SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, alpha);
            }

            if(hasLight)
            {
                light2D.intensity = Lcurve.Evaluate(Timer / duration);
            }
            
            Timer -= Time.deltaTime;

            yield return new WaitForSeconds(0f);
        }
        
        Destroy(gameObject);
    }

}
