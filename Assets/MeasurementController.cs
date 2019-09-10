using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastManager))]
public class MeasurementController : MonoBehaviour
{
    [SerializeField]
    private GameObject linePrefab;

    [SerializeField]
    private GameObject measurementPointPrefab;

    [SerializeField]
    private ARCameraManager arCameraManager;
    
    private LineRenderer measureLine;

    private ARRaycastManager arRaycastManager;

    private GameObject startPoint;

    private GameObject endPoint;

    private Vector2 touchPosition = default;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake() 
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        
        startPoint = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity);
        endPoint = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity);
        
        startPoint.SetActive(false);
        endPoint.SetActive(false);
    }

    private void OnEnable() 
    {
        if(measurementPointPrefab == null)
        {
            Debug.Log("measurementPointPrefab must be set");
            enabled = false;
        }    
    }
    
    void Update()
    { 
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                touchPosition = touch.position;

                if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    startPoint.SetActive(true);

                    Pose hitPose = hits[0].pose;
                    startPoint.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                }
            }

            if(touch.phase == TouchPhase.Moved)
            {
                touchPosition = touch.position;
                
                if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    endPoint.SetActive(true);

                    Pose hitPose = hits[0].pose;
                    endPoint.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                }
            }

            if(touch.phase == TouchPhase.Ended)
            {
                Debug.Log("You removed your finger from the screen");
                Debug.Log("StartPoint " + "x: " + startPoint.transform.position.x + " y: " + startPoint.transform.position.y);
                Debug.Log("EndPoint " + "x: " + endPoint.transform.position.x + " y: " + endPoint.transform.position.y);
            }
        }
        
    }
}
