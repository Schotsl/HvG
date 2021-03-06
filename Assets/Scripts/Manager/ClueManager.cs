using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ClueWrapper
{
    public bool clueFound;
    public string clueName;
    public string cluePrevious;

    public GameObject clueLine;
    public GameObject clueObject;

    public List<GameObject> clueChildren = new List<GameObject>();
}

public class ClueManager : MonoBehaviour
{

    private GameObject websocketObject;
    private WebsocketManager websocketScript;

    [NonReorderable]
    public List<ClueWrapper> clueList;

    public Animator clueContainer;
    public Animator clueNotification;

    public GameObject clueObject;
    public GameObject overTrigger;
    public GameObject endingTrigger;

    public TextMeshProUGUI clueName;
    public TextMeshProUGUI clueContent;

    public AudioSource clueSSound;
    public AudioSource clueMSound;
    public AudioSource pling;

    void Start()
    {
        websocketObject = GameObject.Find("WebsocketManager");
        websocketScript = websocketObject.GetComponent<WebsocketManager>();

        // Disable every clue point and line
        clueList.ForEach(
            (clueItem) =>
            {
                clueItem.clueObject.SetActive(false);

                if (clueItem.clueLine)
                {
                    clueItem.clueLine.SetActive(false);
                }
            }
        );

        // Only show to the first clue
        clueList[0].clueObject.SetActive(true);

        // Enable the other helper classes
        clueObject.SetActive(true);
        clueContainer.SetBool("isOpen", false);

        // Listen for clues from the other player
        websocketScript.AddClue(ReceiveClue);
    }

    public void ReceiveClue(string target) 
    {
        FoundClue(target, false);
    }

    public void FoundClue(string target, bool received = false, bool notification = true)
    {
        if (target == "ClueM3") Globals.isHobod = true;
        if (target == "ClueM5") Globals.isPolice = true;
        if (target == "ClueM6") {
            overTrigger.SetActive(true);
            endingTrigger.SetActive(true);
        }

        // We'll always pass the clue along if even we've already found it
        if (!received) {
            ClueUpdate update = new ClueUpdate(target);

            // Send the new clue to the other player
            websocketScript.SendWebsocket(update);
        }

        ClueWrapper clueWrapper = clueList.Find(clueWrapper => clueWrapper.clueName == target);

        if (!clueWrapper.clueFound)
        {
            if (clueWrapper.cluePrevious != "") {
                FoundClue(clueWrapper.cluePrevious, true, false);
            }

            GameObject clueLine = clueWrapper.clueLine;
            GameObject clueObject = clueWrapper.clueObject;

            clueWrapper.clueFound = true;

            if (notification) {
                StartCoroutine(ClueNotification());
                if (target.Substring(0, 5) == "ClueM") {
                    clueMSound.Play();
                } else if (target.Substring(0, 5) == "ClueS") {
                    clueSSound.Play();
                }
            }
            
            // We can always remove the question mark
            clueObject.transform.Find("?").gameObject.SetActive(false);

            if (clueObject.transform.Find("Image") != null)
            {
                // If there is a photo we'll switch too that
                clueObject.transform.Find("Image").gameObject.SetActive(true);

                // We only need to expand the icon if it's a main clue
                clueObject.transform.localScale += new Vector3(0.9f, 0.9f, 0);
            }
            else
            {
                // If there is no photo we'll switch to the other text
                clueObject.transform.Find("!").gameObject.SetActive(true);
            }

            if (clueLine)
            {
                clueWrapper.clueLine.SetActive(true);
            }

            List<GameObject> clueChildren = clueWrapper.clueChildren;

            clueChildren.ForEach(
                (clueChild) =>
                {
                    clueChild.SetActive(true);
                }
            );
        }
    }

    public void GetClue(ClueDisplay clueDialogue, string target)
    {
        ClueWrapper clueWrapper = clueList.Find(clueWrapper => clueWrapper.clueName == target);

        // Enable the clue container for the text and title
        clueObject.SetActive(true);
        clueContainer.SetBool("isOpen", true);

        bool clueFound = clueWrapper.clueFound;

        // Fill the clue container with the text and title
        clueName.text = clueFound ? clueDialogue.name : "???";
        clueContent.text = clueFound ? clueDialogue.sentence : "???";
        pling.Play();
    }

    public void EndDialogue()
    {
        clueContainer.SetBool("isOpen", false);
    }

    IEnumerator ClueNotification() {
        clueNotification.SetBool("isOpen", true);
        
        yield return new WaitForSeconds(2);
        
        clueNotification.SetBool("isOpen", false);
    }
}
