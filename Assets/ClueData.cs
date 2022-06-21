using UnityEngine;

public class ClueData : MonoBehaviour
{
    public string clueName;
    public ClueDisplay clueDialogue;

    public void TriggerClue()
    {
            FindObjectOfType<ClueManager>().GetClue(clueDialogue, clueName);               
    }
}
