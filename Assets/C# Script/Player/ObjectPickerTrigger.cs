using UnityEngine;

public class ObjectPickerTrigger : MonoBehaviour
{
    public bool isTriggered = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PickableObject"))
        {
            isTriggered = true;
        }else
        {
            isTriggered = false;
        }
    }

}
