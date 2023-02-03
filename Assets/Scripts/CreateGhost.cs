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

    void Update()
    {
        if(!TryGetTouchPosition(out Vector2 touchPosition)){
            return;
        }
        if(_arRaycastManager.Raycast(touchPosition, hits, trackableTypes: TrackableType.PlaneWithinPolygon))
        {

            var hitPose = hits[0].pose;

            if(SpawnedGhost == null)
            {
                SpawnedGhost = Instantiate(ghost, hitPose.position, hitPose.rotation);
            }
            else
            {
                SpawnedGhost.transform.position = hitPose.position;
            }
        }
        SpawnedGhost.transform.LookAt(Camera.main.transform);

    }
}
