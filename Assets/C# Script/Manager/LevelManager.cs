using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static int no_of_functionScene
    {
        get
        {
            return 4; //Start, Menu, Setting, Level
        }
    }
    public static int no_of_level
    {
        get
        {
            return SceneManager.sceneCountInBuildSettings - no_of_functionScene;
        }
    }

    public static int currentLevel
    {
        get
        {
            return SceneManager.GetActiveScene().buildIndex - no_of_functionScene + 1;
        }
    }

    [Header("LevelSceneSettings")]
    public RectTransform content;
    public GameObject levelButton;


    private void Start()
    {      

        if (content == null)
            return;

        //Destroy previous Buttons
        foreach (Button button in content.GetComponentsInChildren<Button>())
        {
            Destroy(button.gameObject);
        }

        //Create Buttons
        int LevelReached = PlayerPrefs.GetInt("LevelReached");
        if(PlayerPrefs.GetInt("LevelReached") == 0)
        {
            PlayerPrefs.SetInt("LevelReached", 1);
            LevelReached = PlayerPrefs.GetInt("LevelReached");
        }
        for (int i = 1; i <= no_of_level; i++)
        {
            GameObject button = (GameObject)Instantiate(levelButton, content);
            button.name = "LevelButton" + "(" + i + ")";

            TextMeshProUGUI textComponent = button.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = i.ToString();

            Button buttonComponent = button.GetComponent<Button>();
            int levelIndex = i + no_of_functionScene - 1;
            buttonComponent.onClick.AddListener(delegate { ButtonClick(levelIndex); });
            buttonComponent.onClick.AddListener(delegate { AudioManager.instance.StopAudio("BGM", SceneLoadingManager.instance.fadeTime - Time.deltaTime); });

            if(i <= LevelReached)
            {
                buttonComponent.interactable = true;
            }else
            {
                buttonComponent.interactable = false;
            }
        }
    }

    void ButtonClick(int index)
    {
        SceneLoadingManager.instance.SpecificScene_Int(index);
    }

    public void ResetPrefs(GameObject sign)
    {
        PlayerPrefs.SetInt("LevelReached", 1);
        sign.SetActive(true);
    }
}
