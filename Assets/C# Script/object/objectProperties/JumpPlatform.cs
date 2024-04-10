using UnityEngine;

public class JumpPlatform : MonoBehaviour, IDataTransferable<GameObject>
{
    public Vector2 bounceForce;
    private Animator ani;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ani = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb_player = player.GetComponent<Rigidbody2D>();
             
                if ((rb_player.velocity.y < 0.05f && rb_player.velocity.y > -0.05f) || isPickedObject)
                    return;

                rb_player.velocity = new Vector2(rb_player.velocity.x, 0f);
                rb_player.AddForce(bounceForce);

                ani.SetTrigger("bounced");

                AudioManager.instance.PlayAudio("bounce");

            
        }
        
    }

    private bool isPickedObject
    {
        get
        {
            return gameObject == player.GetComponent<ObjectPicker>().pickedObject;
        }
    }

    public GameObject Send()
    {
        return gameObject;
    }

    public void Recieve(GameObject data)
    {
        JumpPlatform jp = data.GetComponent<JumpPlatform>();
        bounceForce = jp.bounceForce;
    }
}

