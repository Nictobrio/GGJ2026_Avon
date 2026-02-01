using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueDatabase dialogues;

    void Start()
    {
        TriggerDialogue(DialogueStep.Greeting);   
    }

    public void TriggerDialogue(DialogueStep step)
    {
        DialogueManager.instance.StartDialogue(dialogues.DialogueDict[step]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

}
