using UnityEngine;

public class ClueTrigger : MonoBehaviour
{
    public GameObject canClick;

    private bool isNear;

    public string clueNumber;

    private bool clueFound;

    public void TriggerClue()
    {
        if (isNear) {
            FindObjectOfType<ClueManager>().FoundClue(clueNumber);
            clueFound = true;
            canClick.SetActive(false);
            isNear = false;
        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player" && clueFound == false) 
        {
            canClick.SetActive(true);
            isNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "Player" && clueFound == false)
        {
            canClick.SetActive(false);
            isNear = false;
        }
    }
}
