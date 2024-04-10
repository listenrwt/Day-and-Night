using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public float delayRate;
    public Vector3 offset;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        transform.position = (Vector3)Vector2.Lerp(transform.position, player.position, delayRate) + offset;
    }
}
