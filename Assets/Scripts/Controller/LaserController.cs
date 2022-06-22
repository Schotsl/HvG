using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserController : MonoBehaviour
{
    private GameObject websocketObject;
    private WebsocketManager websocketScript;

    private bool laserTouching;
    public List<GameObject> laserList;

    // Start is called before the first frame update
    void Start()
    {   
        websocketObject = GameObject.Find("WebsocketManager");
        websocketScript = websocketObject.GetComponent<WebsocketManager>();

        websocketScript.AddLaser(RecieveLaser);
    }

    public void ButtonReset() {
        laserList.ForEach((laserObject) => {
            laserObject.SetActive(true);
        });
    }

    public void ButtonPressed(string buttonName) {
        ButtonReset();

        if (buttonName == "Button #1") {
            InstructLaser(laserList[2], false);
            InstructLaser(laserList[5], false);
        } else if (buttonName == "Button #2") {
            InstructLaser(laserList[2], false);
            InstructLaser(laserList[4], false);
        } else if (buttonName == "Button #3") {
            InstructLaser(laserList[1], false);
            InstructLaser(laserList[5], false);
        }
    }

    public void InstructLaser(GameObject laserObject, bool laserState) {
        string laserTarget = laserObject.name;

        LaserUpdate update = new LaserUpdate(laserTarget, laserState);
        websocketScript.SendWebsocket(update);

        laserObject.SetActive(laserState);
    }

    public void RecieveLaser(string laserName, bool laserState) {
        laserList.ForEach((laserObject) => {
            if (laserObject.name == laserName) {
                laserObject.SetActive(laserState);

                LaserUpdate update = new LaserUpdate(laserTarget, laserState);
                websocketScript.SendWebsocket(update);
            }
        });
    }
}
