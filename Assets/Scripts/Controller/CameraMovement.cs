using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour

{
    private MoveControls controls;

    [SerializeField]

    public Camera cam;

    [SerializeField]

    private float zoomStep, minCamSize, maxCamSize; 

    [SerializeField]

    private SpriteRenderer mapRenderer;

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    private Vector3 dragOrigin;
    private Vector2 mousePos;

    private float mouseScroll;
    private Coroutine zoomCoroutine;

    // private float speed = 4f;

    // used this tutorial https://www.youtube.com/watch?v=R6scxu1BHhs
    // and this for pinch https://www.youtube.com/watch?v=5LEVj3PLufE&t=466s
    private void Awake()
    {
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
        controls = new MoveControls();
        controls.Mouse.MouseWheel.performed += x => mouseScroll = x.ReadValue<float>();
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }

    // Update is called once per frame
    void Update(){
        PanCamera();
        if(mouseScroll > 0){
            ZoomIn();
        } else if(mouseScroll < 0) {
            ZoomOut();
        }
    }

    private void PanCamera(){
        if (controls.Mouse.LeftClickDown.triggered){
            dragOrigin = cam.ScreenToWorldPoint(controls.Mouse.MousePos.ReadValue<Vector2>());
        }
        if (controls.Mouse.LeftClickDown.inProgress)
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(controls.Mouse.MousePos.ReadValue<Vector2>());
            cam.transform.position = ClampCamera(cam.transform.position + difference);   
        }
    }

    IEnumerator ZoomDetection(){
        float previousDistance = 0f, distance = 0f;
        while(true) {
            distance = Vector2.Distance(controls.Touch.PrimaryFingerPos.ReadValue<Vector2>(), controls.Touch.SecondaryFingerPos.ReadValue<Vector2>());
            Debug.Log(distance);
            if (distance > previousDistance) {
                ZoomIn();
                Debug.Log("Zooming in");
            } else if (distance < previousDistance){
                ZoomOut();
                Debug.Log("Zooming out");
            }
        }
    }

    public void ZoomIn(){
        float newSize = cam.orthographicSize - zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
        cam.transform.position = ClampCamera(cam.transform.position);  
    }

    public void ZoomOut(){
        float newSize = cam.orthographicSize + zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
        cam.transform.position = ClampCamera(cam.transform.position); 
    }



    private Vector3 ClampCamera(Vector3 targetPosition){
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;
        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;
        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);
        return new Vector3(newX, newY, targetPosition.z);
    }
}