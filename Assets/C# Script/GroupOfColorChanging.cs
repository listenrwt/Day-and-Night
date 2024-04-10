using UnityEngine;

[ExecuteInEditMode]
public class GroupOfColorChanging : MonoBehaviour
{
    private ColorChanging[] cc;
    public Color color;
    private void Update()
    {
        cc = gameObject.GetComponentsInChildren<ColorChanging>();
        foreach (ColorChanging c in cc)
        {
            c.color = color;
        }
    }
}
