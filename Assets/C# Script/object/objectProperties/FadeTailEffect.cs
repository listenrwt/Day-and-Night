using UnityEngine;

public class FadeTailEffect : MonoBehaviour
{
    [Header("tail")]
    public GameObject tail;
    public float spawnTime;
    private float timer;

    private void Start()
    {
        timer = spawnTime;
    }

    private void Update()
    {
        SpawnTail();
    }

    void SpawnTail()
    {
        if(TimeManager.instance != null)
        if (TimeManager.instance.ActionDisable)
            return;

        if (timer <= 0)
        {
            Instantiate(tail, transform.position, transform.rotation);
            timer = spawnTime;
        }
        else
        {           
            timer -= Time.deltaTime;
        }
    }

}
