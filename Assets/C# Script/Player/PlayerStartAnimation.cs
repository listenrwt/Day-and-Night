using UnityEngine;

public class PlayerStartAnimation : MonoBehaviour
{
    public Transform startpt;
    public Transform endpt;

    public float fallingSpeed;
    public float rotateSpeed;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 dir = transform.position - endpt.position;
        rb.velocity = dir.normalized * fallingSpeed;
        transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, rotateSpeed * Time.deltaTime));

        if(Vector2.Distance(transform.position, endpt.position) <= 0.5)
        {
            transform.position = startpt.position;
        }
    }
}
