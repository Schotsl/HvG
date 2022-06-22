using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // Made using https://www.youtube.com/watch?v=_nRzoTzeyxU

    private Queue<string> sentences;

    private bool isTyping;
    private string completeText;

    public GameObject dialogueObject;

    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueContent;

    public Animator animator;

    private string clueNumber;
    private string altClueNumber;

    //werkt niet helemaal
    private bool hasTalked;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, Dialogue altDialogue, bool extendDialogue, bool hasTalked, string clueNumber = "", string altCluenumber = "")
    {            
        dialogueObject.SetActive(true);
        animator.SetBool("isOpen", true);
        this.clueNumber = clueNumber;
        this.altClueNumber = altCluenumber;
        this.hasTalked = hasTalked;
        Globals.isDialoguing = true;
        dialogueName.text = dialogue.name;
        sentences.Clear();

        if (!this.hasTalked || !extendDialogue) {
            
            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

        } else if (extendDialogue) {

            foreach (string sentence in altDialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
        }

            DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping == true)
        {
            CompleteText();
            StopAllCoroutines();
            isTyping = false;
            return;
        }
        if (sentences.Count == 0)
        {
            EndDialogue();
            Globals.isDialoguing = false;
            return;
        }
        string sentence = sentences.Dequeue();
        completeText = sentence;
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueContent.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueContent.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
    }

    private void CompleteText()
    {
        dialogueContent.text = completeText;
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        if (this.clueNumber != "" && !this.hasTalked) {
            FindObjectOfType<ClueManager>().FoundClue(this.clueNumber);
        } 
        else if (this.altClueNumber != "" && this.hasTalked) {
            FindObjectOfType<ClueManager>().FoundClue(this.altClueNumber);
        }

    }
}
