using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueData : MonoBehaviour
{
    public ClueDisplay clueDialogue;

    public void TriggerClue()
    {
        FindObjectOfType<ClueManager>().GetClue(clueDialogue);
    }
}
