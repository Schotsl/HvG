using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public bool extendDialogue = false;

    public Dialogue altDialogue;

    public GameObject canTalk;

    public string clueNumber;
    public string altCluenumber;

    private bool isNear;

    private bool hasTalked;

    void Start() {
        hasTalked = false;
    }

    public void TriggerDialogue()
    {
        if (isNear) {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, altDialogue, extendDialogue, hasTalked, clueNumber, altCluenumber);
            hasTalked = true;
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
