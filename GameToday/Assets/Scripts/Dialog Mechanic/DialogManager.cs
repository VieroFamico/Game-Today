using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    public Animator animator;
    [Header("Texts")]
    //public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;

    private Queue<string> sentences;
    private string sentenceBeingTyped;
    private bool isTyping = false;
    public bool playingDialog = false;
    public UnityEvent dialogEnded;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (dialogEnded == null)
        {
            dialogEnded = new UnityEvent();
        }
    }
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialog(Dialog dialog)
    {
        playingDialog = true;
        animator.SetBool("IsShowing", true);
        //nameText.text = dialog.name;
        if(dialog.stopPlayerDuringDialog)
        {
            StateManager_Player.instance.SetMoving_Dashing_Attacking(false, false, false);
        }

        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        StartCoroutine(DelayUntilStart());
    }
    IEnumerator DelayUntilStart()
    {
        yield return new WaitForSeconds(0f);
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        /*if (isTyping)
        {
            StopAllCoroutines();
            dialogText.text = sentenceBeingTyped;
            isTyping = false;
            sentenceBeingTyped = "";
            return;
        }*/

        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogText.text = "";
        sentenceBeingTyped = sentence;
        foreach (char c in sentence)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
        sentenceBeingTyped = "";

        yield return new WaitForSeconds(3f);

        DisplayNextSentence();
    }
    public void EndDialog()
    {
        dialogText.text = "";
        animator.SetBool("IsShowing", false);

        playingDialog = false;
        
        StateManager_Player.instance.SetMoving_Dashing_Attacking(true, true, true);

        dialogEnded.Invoke();
    }
}
