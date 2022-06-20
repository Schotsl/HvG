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
                bool clueFound = clueItem.clueFound;

                clueItem.clueObject.SetActive(true);

                if (clueItem.clueLine)
                {
                    clueItem.clueLine.SetActive(clueFound);
                }

                if (clueFound)
                {
                    clueItem.clueObject.transform.Find("?").gameObject.SetActive(false);
                    clueItem.clueObject.transform.Find("Image").gameObject.SetActive(true);
                }
            }
        );

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

            clueObject.transform.localScale += new Vector3(0.9f, 0.9f, 0);
            clueObject.transform.Find("?").gameObject.SetActive(false);
            clueObject.transform.Find("Image").gameObject.SetActive(true);

            if (clueLine)
            {
                clueWrapper.clueLine.SetActive(true);
            }
        }
    }

    public void GetClue(ClueDisplay clueDialogue, string temp)
    {
        ClueWrapper clueWrapper = clueList.Find(clueWrapper => clueWrapper.clueName == temp);

        animator.SetBool("isOpen", true);
        clueObject.SetActive(true);

        bool clueFound = clueWrapper.clueFound;

        clueName.text = clueFound ? clueDialogue.name : "???";
        clueContent.text = clueFound ? clueDialogue.sentence : "???";
    }

    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }
}
