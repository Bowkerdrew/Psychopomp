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
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;

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
    }
    void Update()
    {
        cameraPosition = Camera.main.transform.position;
        if (SpawnedGhost == null)
        {
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
        }


        lookCamera();
        checkKill();
        deathCheck();
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
