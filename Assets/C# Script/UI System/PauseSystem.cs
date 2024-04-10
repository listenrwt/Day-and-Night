using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    public static bool isPaused;

    public GameObject PausePanel;

    private void Awake()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (EndGate.passed)
            return;

        if (Input.GetKeyDown(KeyCode.Escape) && !SceneLoadingManager.isFading)
        {
            if (!isPaused)
            {
                Pause();
            }else
            {
                UnPause();
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
        PausePanel.SetActive(true);
        isPaused = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
        isPaused = false;
    }

}
