using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(ARRaycastManager))]
public class CreateGhost : MonoBehaviour
{
    
    TextMeshProUGUI scoreText;
    //TextMeshProUGUI bottleText;
    
    GameObject ScoreBoardUI;
   //GameObject BottleCountUI;
 
   
    public static int score;
   //public static int bottle;
 
    public GameObject ghost;
    public float WaitTime;
    private GameObject SpawnedGhost;
    private GameObject Water;
    public string MiniGame;
    public int deathCount;
    private int kills;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;
    public float minThrowSwipeDistance;
    
    private Vector2 touchStartPosition = Vector2.zero;
     
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject holywater;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey= KeyCode.Mouse0;

    [Range(10f, 70f)]
    public float throwForce;
    public float throwUpwardForce;

    public float destroyDistance = 1f;

    bool readyToThrow;


    private Vector3 cameraPosition;
    public float speed = 0.05f;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    // Update is called once per frame

    private void Awake(){
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }
    bool TryGetTouchPosition(out Vector2 touchPosition){
        
        if(Input.touchCount > 0){
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }
    void Start()
    {
        ScoreBoardUI = GameObject.FindGameObjectWithTag("ScoreCanvas");
        scoreText = GameObject.FindGameObjectWithTag("ScoreOnBanner").GetComponent<TextMeshProUGUI>();
       // BottleCountUI=GameObject.FindGameObjectWithTag("BottlesCanvas");
       // bottleText=GameObject.FindGameObjectWithTag("BottlesAmount").GetComponent<TextMeshProUGUI>();

       
       
        ghost.AddComponent<BoxCollider>();
        
        readyToThrow=true;
        StartCoroutine(Spawner());
        score=0;
        //bottle=5;
        
    }
    void Update()
    {
        
        scoreText.text= "Kills: " + score.ToString();
        //bottleText.text= "Bottles: "  + bottle.ToString();
        
        cameraPosition = Camera.main.transform.position;
        if (bottle == 0)
        {
            SceneManager.LoadScene("Death");
        }
        
        if (SpawnedGhost != null)
        {
            SpawnedGhost.transform.LookAt(Camera.main.transform.position);
            SpawnedGhost.transform.Translate(Vector3.forward * Time.deltaTime * speed);
            lookCamera();
            checkKill();
            deathCheck();
        }
        //if (SpawnedGhost != null && SpawnedGhost.GetComponent<Collider>() == null)
        //{
           // SpawnedGhost.AddComponent<BoxCollider>();
            //SpawnedGhost.GetComponent<BoxCollider>().isTrigger = true;

        //}


       // if(Input.GetKeyDown(throwKey)&& readyToThrow && totalThrows > 0)
       // {
           // Throw();
       // }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
              touchStartPosition = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Vector2 touchEndPosition = Input.GetTouch(0).position;
            if (Vector2.Distance(touchStartPosition, touchEndPosition) >= minThrowSwipeDistance && readyToThrow && totalThrows > 0)
            {
            Throw();
            }
       }

       

    }
        IEnumerator Spawner(){

        if(SpawnedGhost == null){
            yield return new WaitForSeconds(WaitTime);
            spawn();
        }
            

    }
    void spawn(){
                  // if (SpawnedGhost == null)
           // {

            // SpawnedGhost = Instantiate(ghost, Camera.main.transform.position, Camera.main.transform.rotation);
            //SpawnedGhost.AddComponent<BoxCollider>();
            


                Vector3 randposition;
                //randposition.x = cameraPosition.x + 1;
                //randposition.y = cameraPosition.y +1;
                //randposition.z = cameraPosition.z;
                float distanceFromCamera = 5f; 
                randposition = cameraPosition + Camera.main.transform.forward * distanceFromCamera;
                SpawnedGhost = Instantiate(ghost, randposition, Camera.main.transform.rotation);
                SpawnedGhost.AddComponent<BoxCollider>(); 
                lookCamera();

       // }
    }
    void Throw()
    {
       // if (bottle > 0  ){
         Debug.Log("Throw() function called.");
   

        readyToThrow = false;
        
        GameObject projectile = Instantiate(holywater,cameraPosition,  Camera.main.transform.rotation);
       
        Debug.Log("Projectile instantiated.");

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceToAdd = Camera.main.transform.forward * throwForce+ transform.up*throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;
        //bottle--;
        //bottleText.text= "Bottles: "  + bottle.ToString();
        Invoke(nameof(ResetThrow), throwCooldown);
        //checkKill();
        //}
        
    } 

    void ResetThrow()
    {
        readyToThrow=true;
    }

   

    void lookCamera(){
        Vector3 ghostPosition = SpawnedGhost.transform.position;
        ghostPosition.y = 0f;
        cameraPosition.y = 0f;
        Vector3 direction = cameraPosition - ghostPosition;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        SpawnedGhost.transform.rotation = targetRotation;

    }



     void checkKill(){
                    //  Vector3 ghostPosition = SpawnedGhost.transform.position;
                    // Vector3 waterPosition = Water.transform.position;
                    //  if (Vector3.Distance(waterPosition, ghostPosition) < 0.1)
                    //  {
                    //      Destroy(SpawnedGhost);
                    //      SpawnedGhost = null;
                    //  }
        Collider[] hitColliders = Physics.OverlapSphere(SpawnedGhost.transform.position, destroyDistance);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject != SpawnedGhost){
                Destroy(SpawnedGhost);
                SpawnedGhost = null;
                kills++;
                winCheck();
                StartCoroutine(Spawner());
               score++;
               scoreText.text = "Kills: " + score.ToString();
            }
            
        }
        

     }
    void winCheck(){
        if(kills >= deathCount){
            SceneManager.LoadScene(MiniGame);
        }

    }
    void deathCheck(){
        if(SpawnedGhost != null && Vector3.Distance(SpawnedGhost.transform.position, cameraPosition) < 0.2){
            score=0;
            SceneManager.LoadScene("Death");
        }
        
    }
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("holywater"))
        {
           float distance = Vector3.Distance(transform.position, other.transform.position);
            if (distance < destroyDistance)
            {
                Destroy(SpawnedGhost);
            }
        }
       
        
      
    }
     
    
    
}

    




