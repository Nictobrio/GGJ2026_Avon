using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAsync;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private List<Dialogue> dialogues;

    private Transform target;

    public void TriggerDialogue(string lastStage)
    {
        foreach (var dialogue in dialogues)
        {
            if (dialogue.Step.Equals(lastStage))
            {
                DialogueManager.instance.StartDialogue(dialogue);
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) target = collision.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

}
