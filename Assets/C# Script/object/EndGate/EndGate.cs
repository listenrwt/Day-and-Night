using UnityEngine.SceneManagement;
using UnityEngine;

public class EndGate : MonoBehaviour, IDataTransferable<GameObject>
{
    [Header("Animation")]
    public float rotaSpeed; 

    [Header("PassPanel Settings")]
    public GameObject PassPanel;
    public GameObject NextLevelButton;
    public static bool passed = false;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimator>().endgate = transform;
        passed = false;
    }

    private void Update()
    {
        if(TimeManager.instance == null)
        {
            transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, rotaSpeed));
            return;
        }

        if (TimeManager.instance.ActionDisable)
            return;

        transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, rotaSpeed));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (TimeManager.instance == null)
            return;

        if (collision.CompareTag("Player") && !passed && !TimeManager.instance.ActionDisable)
        {
            passed = true;
            GamePass();           
        }
    }

    private void GamePass()
    {
        PassPanel.SetActive(true);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(0f, 0f);
        rb.freezeRotation = false;
        player.GetComponent<PlayerMovement>().enabled = false;

        AudioManager.instance.PlayAudio("teleport");

        if (LevelManager.currentLevel == PlayerPrefs.GetInt("LevelReached"))
            PlayerPrefs.SetInt("LevelReached", PlayerPrefs.GetInt("LevelReached") + 1);

        if (LevelManager.currentLevel >= LevelManager.no_of_level)
            NextLevelButton.SetActive(false);

    }

    public GameObject Send()
    {
        return gameObject;
    }

    public void Recieve(GameObject data)
    {
        objectBehaviour ob = data.GetComponent<objectBehaviour>();
        GetComponent<objectBehaviour>().list = ob.list;

        EndGate endgate = data.GetComponent<EndGate>();
        PassPanel = endgate.PassPanel;
        NextLevelButton = endgate.NextLevelButton;

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimator>().endgate = transform;
    }
}
