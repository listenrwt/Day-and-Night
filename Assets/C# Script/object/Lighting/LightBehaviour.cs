using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightBehaviour : objectBehaviour
{
    private Light2D light2D;
    private float oriIntensity = 0f;

    private float fadingTime;

    private void Start()
    {
        if (list != null)
        {
            for (int i = 0; i < list.objects.Length; i++)
            {
                if (list.objects[i].key == key)
                {
                    TimeIndex = i;
                    break;
                }
            }
        }
        childSR = GetComponentsInChildren<SpriteRenderer>();
        fadingTime = TimeManager.instance.enlargingTime;
        light2D = GetComponent<Light2D>();
    }

    protected override void LightFade(GameObject newObj)
    {
        Light2D newlight2D = newObj.GetComponent<Light2D>();

        if (newlight2D == null)
            return;

        if (light2D != null)
            oriIntensity = light2D.intensity;
            light2D.enabled = false;

        newlight2D.enabled = true;

        float newIntensity = newlight2D.intensity;

        if (oriIntensity == newIntensity)
            return;

        newlight2D.intensity = oriIntensity;
        StartCoroutine(lightfading(newlight2D, newIntensity - oriIntensity));
    }

    IEnumerator lightfading(Light2D newlight2d, float errorIntensity)
    {
        float fadingTimer = fadingTime;
        while(fadingTimer > 0)
        {
            float deltaIntensity = errorIntensity / fadingTime * Time.deltaTime;
            newlight2d.intensity += deltaIntensity;

            fadingTimer -= Time.deltaTime;
            yield return new WaitForSeconds(0);
        }

        newlight2d.intensity = errorIntensity + oriIntensity;
    }

    protected override void DisableOldGlobalLight()
    {
        if (light2D.lightType == Light2D.LightType.Global)
            light2D.enabled = false;      
    }

}
