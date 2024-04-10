using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadingSystem : MonoBehaviour
{
    public static FadingSystem system;

    public bool fading = false;

    public Image fadePanel;
    public AnimationCurve AniC;
    public float FadeTime;

    private void Awake()
    {
        if(system != null)
        {
            Destroy(gameObject);
        }else
        {
            system = this;
        }
    }

    public IEnumerator FadeIn()
    {
        fading = true;

        float timer = FadeTime;

        if(timer >= 0f)
        {
           float _alpha  = AniC.Evaluate(timer);
           fadePanel.color = new Color(0f, 0f, 0f, _alpha);

            timer -= Time.deltaTime;
            yield return new WaitForSeconds(0f);
        }

        fading = false;
    
    }

}
