using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    private PlayerInputActions playerInputActions;

    public GameObject mobileUI;
    public GameObject clueMapContainer;
    private GameObject gameUI;

    public AudioSource pling;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        Globals.isInMap = false;

    }

    private void OnEnable() {
        playerInputActions.Enable();
    }

    private void OnDisable() {
        playerInputActions.Disable();
    }

    public void cameraToggle(){
        Globals.isInMap = !Globals.isInMap;

        UpdateCamera(Globals.isInMap);
    }

    public void UpdateCamera(bool inMap){
        cam1.SetActive(!Globals.isInMap);
        cam2.SetActive(Globals.isInMap);
        pling.Play();

        if (SystemInfo.deviceType == DeviceType.Handheld) {
            mobileUI.SetActive(!Globals.isInMap);
        }

        FindObjectOfType<ClueManager>().EndDialogue();
        clueMapContainer.GetComponent<UnityEngine.EventSystems.BaseRaycaster>().enabled = Globals.isInMap;    
        
        foreach (GameObject gameUI in GameObject.FindGameObjectsWithTag("gameUI")) {
            gameUI.GetComponent<UnityEngine.EventSystems.BaseRaycaster>().enabled = !Globals.isInMap;        
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInputActions.Player.ClueMapView.triggered){
            cameraToggle();
        }
            
    }
}
