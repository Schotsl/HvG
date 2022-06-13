using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //https://www.youtube.com/watch?v=_nRzoTzeyxU
    private Queue<string> sentences;
    
    public TextMeshProUGUI  CharTxt;
    public TextMeshProUGUI  DialogueTxt;

    public Animator animator;
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue (Dialogue dialogue) {
        Debug.Log("Starting conversation with " + dialogue.name);

        animator.SetBool("isOpen", true);

        CharTxt.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence () {
        if (sentences.Count == 0){
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        Debug.Log(sentence);
    }

    IEnumerator TypeSentence (string sentence) {
        DialogueTxt.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            DialogueTxt.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    void EndDialogue (){
        Debug.Log("End of Conversation");
        animator.SetBool("isOpen", false);
    }
}
