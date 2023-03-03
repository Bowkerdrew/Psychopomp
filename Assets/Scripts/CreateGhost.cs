using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ARRaycastManager))]
public class CreateGhost : MonoBehaviour
{

    public GameObject ghost;
    private GameObject SpawnedGhost;
    public int deathCount;
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
    public float throwForce;
    public float throwUpwardForce;

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
        
        ghost.AddComponent<BoxCollider>();
        readyToThrow=true;
        
    }
    void Update()
    {
        cameraPosition = Camera.main.transform.position;
        if (SpawnedGhost == null)
        {

            SpawnedGhost = Instantiate(ghost, Camera.main.transform.position, Camera.main.transform.rotation);
            SpawnedGhost.AddComponent<BoxCollider>();

            Vector3 randposition;
            randposition.x = cameraPosition.x + 1;
            randposition.y = cameraPosition.y +1;
            randposition.z = cameraPosition.z;
            SpawnedGhost = Instantiate(ghost, randposition, Camera.main.transform.rotation);

            lookCamera();
        }

        if (SpawnedGhost != null)
        {
            SpawnedGhost.transform.LookAt(Camera.main.transform.position);
            SpawnedGhost.transform.Translate(Vector3.forward * Time.deltaTime * speed);

          



            lookCamera();
           
            checkKill();
            deathCheck();
        }
        if (SpawnedGhost != null && SpawnedGhost.GetComponent<Collider>() == null)
        {
            SpawnedGhost.AddComponent<BoxCollider>();
        }

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
    void Throw()
    {
         Debug.Log("Throw() function called.");
   

        readyToThrow = false;
        

        GameObject projectile = Instantiate(holywater,attackPoint.position, cam.rotation);
       
        Debug.Log("Projectile instantiated.");

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceToAdd = cam.transform.forward * throwForce+ transform.up*throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        Invoke(nameof(ResetThrow), throwCooldown);
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

         if (Input.touchCount > 0)
         {
             Touch touch = Input.GetTouch(0);
             if (touch.phase == TouchPhase.Began)
             {
                 List<ARRaycastHit> hits = new List<ARRaycastHit>();
                 if (_arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                 {
                     Pose hitPose = hits[0].pose;
                     Vector3 ghostPosition = SpawnedGhost.transform.position;

                     if (Vector3.Distance(hitPose.position, ghostPosition) < 0.1f)
                     {
                         Destroy(SpawnedGhost);
                         SpawnedGhost = null;
                     }
                 }
             }

         }  
     }

    void deathCheck(){
        if(Vector3.Distance(SpawnedGhost.transform.position, cameraPosition) < 0.1){
            SceneManager.LoadScene("Death");
        }
    }



}
