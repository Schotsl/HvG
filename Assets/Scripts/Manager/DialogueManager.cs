using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue (Dialogue dialogue) {
        dialogueObject.SetActive(true);

        animator.SetBool("isOpen", true);

        dialogueName.text = dialogue.name;

        sentences.Clear();

        FindObjectOfType<PlayerController>().LockMovement();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence () {
        if (isTyping == true) {
            CompleteText();
            StopAllCoroutines();
            isTyping = false;
            return;
        }        
        if (sentences.Count == 0){
            EndDialogue();
            FindObjectOfType<PlayerController>().UnlockMovement();
            return;
        }
        string sentence = sentences.Dequeue();
        completeText = sentence;
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence) {
        isTyping = true;
        dialogueContent.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            dialogueContent.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
    }

    private void CompleteText(){
        dialogueContent.text = completeText;
    }

    void EndDialogue (){
        animator.SetBool("isOpen", false);
    }
}
