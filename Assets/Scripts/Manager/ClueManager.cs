using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    public GameObject clueM1;
    public GameObject clueM2;
    public GameObject clueM2_5;
    public GameObject clueM3;
    public GameObject clueM4;
    public GameObject clueM5;
    public GameObject clueM6;
    public GameObject clueM7;
    public GameObject clueS1;
    public GameObject clueS2;
    public GameObject clueS3;
    public GameObject clueS4;

    public GameObject lineM1;
    public GameObject lineM2;
    public GameObject lineM2_5;
    public GameObject lineM3;
    public GameObject lineM4;
    public GameObject lineM5;
    public GameObject lineM6;
    public GameObject lineS1;
    public GameObject lineS2;
    public GameObject lineS3;
    public GameObject lineS4;

    public bool clue1;
    public bool clue2;    
    public bool clue2_5;
    public bool clue3;
    public bool clue4;
    public bool clue5;
    public bool clue6;
    public bool clue7;
    public bool clueSide1;
    public bool clueSide2;
    public bool clueSide3;
    public bool clueSide4;
    private float clues = 0;
    private bool fake = false;
    private bool clueSide1Found = false;
    private bool clueSide2Found = false;
    private bool clueSide3Found = false;
    private bool clueSide4Found = false;

    // Start is called before the first frame update
    void Start()
    {
        clue1 = false;
        clue2 = false;
        clue2_5 = false;
        clue3 = false;
        clue4 = false;
        clue5 = false;
        clue6 = false;
        clue7 = false;
        clueSide1 = false;
        clueSide2 = false;
        clueSide3 = false;
        clueSide4 = false;

        lineM1.gameObject.SetActive(false);
        lineM2.gameObject.SetActive(false);
        lineM2_5.gameObject.SetActive(false);
        lineM3.gameObject.SetActive(false);
        lineM4.gameObject.SetActive(false);
        lineM5.gameObject.SetActive(false);
        lineM6.gameObject.SetActive(false);
        lineS1.gameObject.SetActive(false);
        lineS2.gameObject.SetActive(false);
        lineS3.gameObject.SetActive(false);
        lineS4.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        NextClue();
    }

    void NextClue() {
        //Start with 1 clue (given)
        //when player finds clue (call this function)
        //in function, if player only has clue 1, add clue 2
        //if player has fake clue, add clue 2.5
        //if player has clue 2, add clue 3
        //............        
        if (clue1 && clues == 0) {
            //if clue1, 
            clueM1.gameObject.transform.localScale += new Vector3(0.9f, 0.9f, 0);
            clueM2.gameObject.SetActive(true);
            clueS1.gameObject.SetActive(true);
            clueS2.gameObject.SetActive(true);
            clueS3.gameObject.SetActive(true);
            clues++;
        }
        if (clue2 && clues == 1) {
            clueM2.gameObject.transform.localScale += new Vector3(0.9f, 0.9f, 0);
            clueM3.gameObject.SetActive(true);
            clueM2_5.gameObject.SetActive(true);
            clueS4.gameObject.SetActive(true);
            lineM1.gameObject.SetActive(true);
            clues++;
        }
        if (clue2_5 && fake == false) {
            clueM2_5.gameObject.transform.localScale += new Vector3(0.9f, 0.9f, 0);
            lineM2_5.gameObject.SetActive(true);
            fake = true;
        }
        if (clue3 && clues == 2) {
            clueM3.gameObject.transform.localScale += new Vector3(0.9f, 0.9f, 0);
            clueM4.gameObject.SetActive(true);
            lineM2.gameObject.SetActive(true);
            clues++;
        } 
        if (clue4 && clues == 3) {
            clueM4.gameObject.transform.localScale += new Vector3(0.9f, 0.9f, 0);
            clueM5.gameObject.SetActive(true);
            lineM3.gameObject.SetActive(true);
            clues++;
        }
        if (clue5 && clues == 4) {
            clueM5.gameObject.transform.localScale += new Vector3(0.9f, 0.9f, 0);
            clueM6.gameObject.SetActive(true);
            lineM4.gameObject.SetActive(true);
            clues++;
        }
        if (clue6 && clues == 5) {
            clueM6.gameObject.transform.localScale += new Vector3(0.9f, 0.9f, 0);
            clueM7.gameObject.SetActive(true);
            lineM5.gameObject.SetActive(true);
            clues++;
        }
        if (clue7 && clues == 6) {
            clueM7.gameObject.transform.localScale += new Vector3(0.9f, 0.9f, 0);
            lineM6.gameObject.SetActive(true);
            clues++;
        }
        if (clueSide1 && clueSide1Found == false) {
            lineS1.gameObject.SetActive(true);
            clueSide1Found = true;
        }
        if (clueSide2 && clueSide2Found == false) {
            lineS2.gameObject.SetActive(true);
            clueSide2Found = true;
        }
        if (clueSide3 && clueSide3Found == false) {
            lineS3.gameObject.SetActive(true);
            clueSide3Found = true;
        }
        if (clueSide4 && clueSide4Found == false) {
            lineS4.gameObject.SetActive(true);
            clueSide4Found = true;
        }          
    }
}
