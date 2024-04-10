using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public CurrentTime time;
    public enum CurrentTime { Day = 1, Night = -1};
    [HideInInspector]
    public bool changingTime = false;

    [Header("Mask Settings")]
    public GameObject mask;
    public float maxScale;
    public float enlargingTime;
    public AnimationCurve enlargeCurve; 
    private bool deleting_mask = false;

    //player
    [HideInInspector]
    public GameObject player;
    private PlayerMovement playerMovement;
    private Rigidbody2D playerRB;
    private CameraFollower playerCam;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    { 

        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerRB = player.GetComponent<Rigidbody2D>();

        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        playerCam = mainCam.GetComponent<CameraFollower>();

        
        if(time == CurrentTime.Day)
        {
            AudioManager.instance.StopAudio("DaySound"); 
            AudioManager.instance.StopAudio("NightSound");
            AudioManager.instance.PlayAudio("DaySound", SceneLoadingManager.instance.fadeTime - Time.deltaTime);
        }else
        {
            AudioManager.instance.StopAudio("DaySound");
            AudioManager.instance.StopAudio("NightSound");
            AudioManager.instance.PlayAudio("NightSound", SceneLoadingManager.instance.fadeTime - Time.deltaTime);
        }
    }

    private bool audioStopped = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !changingTime && !deleting_mask && !PauseSystem.isPaused)
        {
            if (time == CurrentTime.Day)
            {
                AudioManager.instance.StopAudio("DaySound");
                time = CurrentTime.Night;
                StartCoroutine(EnlargingMask());
            }
            else
            {
                AudioManager.instance.StopAudio("NightSound");
                time = CurrentTime.Day;
                StartCoroutine(EnlargingMask());
            }
                
        }

        if (SceneLoadingManager.isFadingIn)
        {
            if (!audioStopped)
            {
                AudioManager.instance.StopAudio("DaySound", SceneLoadingManager.instance.fadeTime - Time.deltaTime);
                AudioManager.instance.StopAudio("NightSound", SceneLoadingManager.instance.fadeTime - Time.deltaTime);
                audioStopped = true;
            }
        }else
        {
            audioStopped = false;
        }
    }

    IEnumerator EnlargingMask()
    {
        changingTime = true;
        playerSettings(false);

        AudioManager.instance.PlayAudio("TimeStop");

        GameObject _mask = (GameObject)Instantiate(mask, player.transform.position, Quaternion.identity);
        Transform maskTransform = _mask.transform;
        
        float enlargingTimer = 0f;
        while (enlargingTimer <= enlargingTime)
        {
            float deltaScale = maxScale * enlargeCurve.Evaluate(enlargingTimer / enlargingTime);
            maskTransform.localScale = new Vector3(deltaScale, deltaScale, 0f);
            enlargingTimer += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
      
        playerSettings(true);
        changingTime = false;      
        StartCoroutine(DeleteMask(_mask));

        if (time == CurrentTime.Day)
        {
            AudioManager.instance.PlayAudio("DaySound");
        }
        else
        {
            AudioManager.instance.PlayAudio("NightSound");
        }

    }

    void playerSettings(bool set)
    {
        if (!set)
        {
            playerRB.gravityScale = 0;
            playerMovement.velocityRegister = playerRB.velocity;
            playerRB.velocity = Vector2.zero;
            playerCam.enabled = false;
        }else
        {
            playerRB.gravityScale = 1;
            playerRB.velocity = playerMovement.velocityRegister;
            playerCam.enabled = true;
        }
    }

    IEnumerator DeleteMask(GameObject mask)
    {
        deleting_mask = true;
        yield return null;
        Destroy(mask);
        deleting_mask = false;
    }

    public bool ActionDisable{ get { return changingTime || PauseSystem.isPaused; } }

}
