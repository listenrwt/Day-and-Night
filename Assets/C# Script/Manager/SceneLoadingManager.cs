using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadingManager : MonoBehaviour
{

    public static SceneLoadingManager instance;

    public static bool isFading;
    public static bool isFadingIn = false;

    private void Start()
    {
        instance = this;
        StartCoroutine(FadeOut());
    }

    public void ReloadScene()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(FadeIn(buildIndex));
    }

    public void NextScene()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(FadeIn(buildIndex));
    }

    public void SpecificScene_Int(int buildIndex)
    {
        StartCoroutine(FadeIn(buildIndex));
    }

    public void SpecificScene_String(string name)
    {
        StartCoroutine(FadeIn(name));  
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #region Fading System

    [Header("Fading System")]
    public Image fadePanel;
    public AnimationCurve AniCurve;
    public float fadeTime;

    IEnumerator FadeOut()
    {
        fadePanel.gameObject.SetActive(true);
        Time.timeScale = 1f;
        isFading = true;

        float timer = fadeTime; 
        while(timer > 0f)
        {
            float alpha = AniCurve.Evaluate(timer);
            fadePanel.color = new Color(0f, 0f, 0f, alpha);

            timer -= Time.deltaTime;
            yield return new WaitForSeconds(0f);
        }

        fadePanel.color = new Color(0f, 0f, 0f, 0f);
        isFading = false;
        fadePanel.gameObject.SetActive(false);
    }

    IEnumerator FadeIn(int buildIndex)
    {
        fadePanel.gameObject.SetActive(true);
        Time.timeScale = 1f;
        isFading = true;
        isFadingIn = true;

        float timer = 0f;
        while (timer < fadeTime)
        {
            float alpha = AniCurve.Evaluate(timer);
            fadePanel.color = new Color(0f, 0f, 0f, alpha);

            timer += Time.deltaTime;
            yield return new WaitForSeconds(0f);
        }

        fadePanel.color = new Color(0f, 0f, 0f, 255f);

        SceneManager.LoadScene(buildIndex);
    }

    IEnumerator FadeIn(string sceneName)
    {
        fadePanel.gameObject.SetActive(true);
        Time.timeScale = 1f;
        isFading = true;

        float timer = 0f;
        while (timer < fadeTime)
        {
            float alpha = AniCurve.Evaluate(timer);
            fadePanel.color = new Color(0f, 0f, 0f, alpha);

            timer += Time.deltaTime;
            yield return new WaitForSeconds(0f);
        }

        fadePanel.color = new Color(0f, 0f, 0f, 255f);

        SceneManager.LoadScene(sceneName);
    }
    #endregion
}
