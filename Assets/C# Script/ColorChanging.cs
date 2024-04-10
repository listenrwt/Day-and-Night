using UnityEngine;

[ExecuteInEditMode]
public class ColorChanging : MonoBehaviour
{
    public Color color;
    public string layer;
    public int orderinlayer;
    public Material material;
    private void Update()
    {
        SpriteRenderer[] childSR = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sr in childSR)
        {
            sr.color = color;
            sr.sortingLayerName = layer;
            sr.sortingOrder = orderinlayer;
            sr.material = material;
        }
    }
}
