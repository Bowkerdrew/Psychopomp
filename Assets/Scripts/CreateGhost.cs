using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


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
        boxCollider.name = "GhostBoxCollider";
    }
    void Update()
    {
        cameraPosition = Camera.main.transform.position;
        if (SpawnedGhost == null)
        {
            SpawnedGhost = Instantiate(ghost, Camera.main.transform.position, Camera.main.transform.rotation);
            SpawnedGhost.AddComponent<BoxCollider>();
            lookCamera();
        }

        if (SpawnedGhost != null)
        {
            SpawnedGhost.transform.LookAt(Camera.main.transform.position);
            SpawnedGhost.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }


    lookCamera();
        //checkKill();
        CallWhenScreenTouched();


    }

    void lookCamera(){
        Vector3 ghostPosition = SpawnedGhost.transform.position;
        ghostPosition.y = 0f;
        cameraPosition.y = 0f;
        Vector3 direction = cameraPosition - ghostPosition;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        SpawnedGhost.transform.rotation = targetRotation;

    }
    void CallWhenScreenTouched()
    {
        var touch = Input.touches[0];
        var ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            BoxCollider collider = hitInfo.collider as BoxCollider;
            if (collider != null)
            {
                collider.name = "MyBoxCollider";
            }
        }
    }


    /* void checkKill(){

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
     }*/
}
