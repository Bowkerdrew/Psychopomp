using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TapDisapear : MonoBehaviour
{
    private void Disapear()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == gameObject)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}