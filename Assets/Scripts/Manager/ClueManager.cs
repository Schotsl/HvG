using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ClueWrapper
{
    public bool clueFound;
    public string clueName;

    public GameObject clueLine;
    public GameObject clueObject;

    public List<GameObject> clueChildren = new List<GameObject>();
}

public class ClueManager : MonoBehaviour
{
    [NonReorderable]
    public List<ClueWrapper> clueList;

    public Animator animator;
    public GameObject clueObject;
    public TextMeshProUGUI clueName;
    public TextMeshProUGUI clueContent;

    // Start is called before the first frame update
    void Start()
    {
        clueList.ForEach(
            (clueItem) =>
            {
                // bool clueFound = clueItem.clueFound;

                // // clueItem.clueObject.SetActive(true);

                clueItem.clueObject.SetActive(false);

                if (clueItem.clueLine)
                {
                    clueItem.clueLine.SetActive(false);
                }

                // if (clueFound)
                // {
                //     clueItem.clueObject.transform.Find("?").gameObject.SetActive(false);
                //     clueItem.clueObject.transform.Find("Image").gameObject.SetActive(true);
                // }
            }
        );

        clueList[0].clueObject.SetActive(true);

        clueObject.SetActive(true);
        animator.SetBool("isOpen", false);
    }

    public void FoundClue(string temp)
    {
        ClueWrapper clueWrapper = clueList.Find(clueWrapper => clueWrapper.clueName == temp);

        if (!clueWrapper.clueFound)
        {
            GameObject clueLine = clueWrapper.clueLine;
            GameObject clueObject = clueWrapper.clueObject;

            clueWrapper.clueFound = true;

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

    public void GetClue(ClueDisplay clueDialogue, string temp)
    {
        ClueWrapper clueWrapper = clueList.Find(clueWrapper => clueWrapper.clueName == temp);

        animator.SetBool("isOpen", true);
        clueObject.SetActive(true);

        bool clueFound = clueWrapper.clueFound;
        Debug.Log(clueFound);
        clueName.text = clueFound ? clueDialogue.name : "???";
        clueContent.text = clueFound ? clueDialogue.sentence : "???";
    }

    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }
}
