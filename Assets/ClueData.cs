using UnityEngine;

public class ClueData : MonoBehaviour
{
    private int count = 0;
    public string clueName;
    public ClueDisplay clueDialogue;

    public void TriggerClue()
    {
        if (clueName != "ClueM1")
        {
            FindObjectOfType<ClueManager>().GetClue(clueDialogue, clueName);
        }
        else
        {
            switch (count)
            {
                case 0:
                    FindObjectOfType<ClueManager>().FoundClue("ClueM1");
                    break;
                case 1:
                    FindObjectOfType<ClueManager>().FoundClue("ClueS1");
                    break;
                case 2:
                    FindObjectOfType<ClueManager>().FoundClue("ClueS2");
                    break;
                case 3:
                    FindObjectOfType<ClueManager>().FoundClue("ClueS3");
                    break;
                case 4:
                    FindObjectOfType<ClueManager>().FoundClue("ClueM2");
                    break;
                case 5:
                    FindObjectOfType<ClueManager>().FoundClue("ClueM2.5");
                    break;
                case 6:
                    FindObjectOfType<ClueManager>().FoundClue("ClueM3");
                    break;
                case 7:
                    FindObjectOfType<ClueManager>().FoundClue("ClueM4");
                    break;
                case 8:
                    FindObjectOfType<ClueManager>().FoundClue("ClueM5");
                    break;
                case 9:
                    FindObjectOfType<ClueManager>().FoundClue("ClueM6");
                    break;
                case 10:
                    FindObjectOfType<ClueManager>().FoundClue("ClueM7");
                    break;
                case 11:
                    FindObjectOfType<ClueManager>().FoundClue("ClueS1");
                    break;
                case 12:
                    FindObjectOfType<ClueManager>().FoundClue("ClueS2");
                    break;
                case 13:
                    FindObjectOfType<ClueManager>().FoundClue("ClueS3");
                    break;
            }

            count++;
        }
    }
}
