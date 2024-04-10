using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public Vector2 pickedOffset = Vector2.zero;
    public Vector3 newScale = Vector3.zero;
    private Vector3 oriScale;

    private ObjectPicker op;

    private void Start()
    {
        gameObject.tag = "PickableObject";
        op = GameObject.FindGameObjectWithTag("Player").GetComponent<ObjectPicker>();
        oriScale = transform.localScale;
    }

    public Vector2 GetPickedOffset()
    {
        return pickedOffset;
    }

    private void Update()
    {
        if (op.pickedObject != this.gameObject)
        {
            transform.localScale = oriScale;
        }
        else
        {
            transform.position = op.SpawnPos.transform.position;
            transform.localScale = newScale;
        } 
    }

}
