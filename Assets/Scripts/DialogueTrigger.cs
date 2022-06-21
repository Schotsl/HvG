using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public GameObject canTalk;

    public string clueNumber;

    private bool isNear;

    public void TriggerDialogue()
    {
        if (isNear) {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, clueNumber);
        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player") 
        {
            canTalk.SetActive(true);
            isNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            canTalk.SetActive(false);
            isNear = false;
        }
    }
}
