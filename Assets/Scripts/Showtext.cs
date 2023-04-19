using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public float displayTime = 1.0f;

    private Text uiText;
    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<Text>();
        StartCoroutine(HideText());
    }
    private IEnumerator HideText()
    {
        yield return new WaitForSeconds(displayTime);
        uiText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
