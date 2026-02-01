using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAsync;

public class DialogueControl : MonoBehaviour
{
    public static DialogueControl instance = null;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject NPC;

    private new Collider2D collider = null;

    private bool inRange;

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        inRange = true;
        collider = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inRange = false;
        collider = null;
    }
}
