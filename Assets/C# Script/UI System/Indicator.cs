using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Indicator : MonoBehaviour
{
    public float fadeTime;
    public AnimationCurve ac;
    public Indicator previousIndicator = null;
    private bool canfade = false;
    private SpriteRenderer[] srs;
    [HideInInspector]
    public bool faded = false;

    private void Start()
    {
        srs = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
        }

        if (previousIndicator == null)
            canfade = true;
    }

    private void Update()
    {
        if(previousIndicator != null)
        canfade = previousIndicator.faded;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !faded && canfade)
        {
            faded = true;
            StartCoroutine(fadeOut());
        }
    }

    IEnumerator fadeOut()
    {
        float timer = 0f;
        while(timer <= fadeTime)
        {
            foreach (SpriteRenderer sr in srs)
            {
                float alpha = ac.Evaluate(timer / fadeTime);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
                timer += Time.deltaTime;
                yield return new WaitForSeconds(0);
            }
        }
    }
}
