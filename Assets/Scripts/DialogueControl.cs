using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAsync;

public class DialogueControl : MonoBehaviour
{
    public static DialogueControl instance = null;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject NPC;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            nextLine();
        }
    }

    private void nextLine()
    {
        if (DialogueManager.instance.NextDialogue.activeSelf)
        {
            DialogueManager.instance.SetNextDialogue();
        }
    }
}
