using UnityEngine;
using TMPro;

public class TextDisappear : MonoBehaviour
{
    public float delayTime = 2f; //time in seconds before the text disappears
    private TextMeshProUGUI text; 

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        Invoke("HideText", delayTime);
    }

    void HideText()
    {
        text.enabled = false;
    }
}
