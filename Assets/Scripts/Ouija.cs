using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ouija : MonoBehaviour
{

    public Image Planchette;
    public GameObject[] locations;

    public string win;
    public float speed;
    private int i = 0;
    public int waittime;
    private bool startpause = false;
    
    // Start is called before the first frame update
    void Start()
    {
       // ;
        StartCoroutine(wait());
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.touchCount > 0 && startpause)
        {
         Planchette.transform.position = Vector3.Lerp(Planchette.transform.position, locations[i].transform.position, speed*Time.deltaTime);
        if(Vector3.Distance(Planchette.transform.position, locations[i].transform.position) < 0.2)
        {
            i++;
        }
        if(i >= locations.Length){
            SceneManager.LoadScene(win);
        }
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 pos = touch.position;
                if(Vector3.Distance(Planchette.transform.position, pos) > 200)
                    {
                        SceneManager.LoadScene("Death");
                    }
            }
            if(touch.phase == TouchPhase.Ended){
                SceneManager.LoadScene("Death");
            }
        }
    }

    IEnumerator wait(){
        yield return new WaitForSeconds(waittime);
        startpause = true;

    }
}