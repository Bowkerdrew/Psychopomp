using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DontDestroyaudio : MonoBehaviour
{
    public AudioSource audioSource;
    void Awake(){
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(transform.gameObject);
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "LevelOne")
        {
            Destroy(gameObject);
        }
    }
}
