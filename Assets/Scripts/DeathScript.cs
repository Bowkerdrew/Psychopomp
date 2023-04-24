using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    public int bottleCount = 3;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (bottleCount <= 0)
        {
            SceneManager.LoadScene("Death");
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bottle"))
        {
            bottleCount--;
        }
    }
}
