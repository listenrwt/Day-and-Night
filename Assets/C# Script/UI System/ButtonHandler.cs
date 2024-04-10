using UnityEngine;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    //Color
    private Color OriginalColor;
    public Color HighlightedColor;
    public Color PressedColor;
    public Color ReleasedColor;

    private TextMeshProUGUI TextMP;

    private void Awake()
    {
        TextMP = GetComponent<TextMeshProUGUI>();
        OriginalColor = TextMP.color;
    }

    private void OnEnable()
    {
        TextMP.color = OriginalColor;
    }

    //Button Animation
    public void Highlighted()
    {
        TextMP.color = HighlightedColor;
    }
    
    public void Left()
    {
        TextMP.color = OriginalColor;   
    }

    public void Pressed()
    {
        TextMP.color = PressedColor;
    }

    public void Released()
    {
        TextMP.color = ReleasedColor;
    }

    //Button Functions
    

}
