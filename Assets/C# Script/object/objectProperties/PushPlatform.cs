using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PushPlatform : MonoBehaviour, IDataTransferable<GameObject>
{
    public float pushForce;
    public bool ignoreSameDir = true;
    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb_player = collision.GetComponent<Rigidbody2D>();
            if (ignoreSameDir && ((pushForce < 0 && rb_player.velocity.x < 0) || (pushForce > 0 && rb_player.velocity.x > 0)))
                return;

            ani.SetTrigger("bounced");
            AudioManager.instance.PlayAudio("bite");

            rb_player.AddForce(new Vector2(pushForce, 0f));          

        }
    }

    public GameObject Send()
    {
        return gameObject;
    }

    public void Recieve(GameObject data)
    {
        PushPlatform pp = data.GetComponent<PushPlatform>();
        pp.pushForce = -pushForce;
    }
}
