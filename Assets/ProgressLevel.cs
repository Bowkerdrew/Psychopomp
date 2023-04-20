using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressLevel : MonoBehaviour
{

    public int nextscene;
    // Start is called before the first frame update
    void Start()
    {
        nextscene = SceneManager.GetActiveScene().buildIndex + 1;
    }

    public void nextsceneload(){
        SceneManager.LoadScene(nextscene);
        if(nextscene > PlayerPrefs.GetInt("levelAt")){
            PlayerPrefs.SetInt("levelAt", nextscene);
        }
    }

}
