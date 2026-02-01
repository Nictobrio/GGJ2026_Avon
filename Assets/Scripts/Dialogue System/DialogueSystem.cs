using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityAsync;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem.Controls;

public class DialogueSystem : MonoBehaviour, ISerializationCallbackReceiver
{
    private bool isInRange, didDialogueStart, fullText, isAnswer, enter, exit;

    public bool optionSelected;

    private string npcName;

    [SerializeField] Dictionary<string, GameObject> npcs;

    [SerializeField] List<GameObject> npcList;

    private Dictionary<int, bool> trigger = new Dictionary<int, bool>();
    int index = 0;

    public Dictionary<string, GameObject> NPCS { get => npcs; set => npcs = value; }

    [SerializeField] private Dialogue dialogues;

    public AnswerType type;

    private Queue<string> textLines;

    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject nextDialogue;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject buttons;


    private Animator DialogueBoxAnimator;
    private float typingTime = 0.03f;

    private float openBoxClipDuration, closeBoxClipDuration;

    Dictionary<AnswerType, List<string>> Answers;

    //Nehuen

    CharacterMovement controller;

    private void Awake()
    {
        textLines = new Queue<string>();
        if (dialogueBox != null) DialogueBoxAnimator = dialogueBox.GetComponent<Animator>();

        if (DialogueBoxAnimator != null)
        {
            openBoxClipDuration =
                DialogueBoxAnimator.runtimeAnimatorController.animationClips.Where(x => x.name.Equals(GameConstants.OPEN_BOX)).FirstOrDefault().length;

            closeBoxClipDuration =
                DialogueBoxAnimator.runtimeAnimatorController.animationClips.Where(x => x.name.Equals(GameConstants.CLOSE_BOX)).FirstOrDefault().length;
        }

        //Nehuen
        controller = GetComponent<CharacterMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isInRange);
        if (isInRange && Input.GetKeyDown(KeyCode.Space) && !isAnswer)
        {
            if (!didDialogueStart)
            {
                StartDialogue();

                //Nehuen
                controller.dialogueStart = true;
            }
            else if (fullText)
            {
                NextDialogueLine();
            }
        }

        if (optionSelected)
        {
            optionSelected = false;
            StartAnswer();
        }

        if (isAnswer && Input.GetKeyDown(KeyCode.Space))
        {
            if (!didDialogueStart)
            {
                StartAnswer();
            }
            else if (fullText)
            {
                NextDialogueLine();
            }
        }
    }

    public async void StartDialogue()
    {
        didDialogueStart = true; 
        textLines.Clear();

        foreach (var item in dialogues.Lines)
        {
            textLines.Enqueue(item.TextLine);
            if (item.TriggerEvent)
            {
                trigger.Add(index, item.TriggerEvent);
            }
            index++;
        }
        index = 0;

        if (!dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(true);
            await new UnityAsync.WaitForSeconds(openBoxClipDuration);
            dialogueText.gameObject.SetActive(true);
        }

        await StartCoroutine(TypeText(textLines.Dequeue(), () => { if (nextDialogue != null) nextDialogue.SetActive(true); fullText = true; }));
    }

    public void StartAnswer()
    {
        didDialogueStart = true;
        textLines.Clear();

        foreach (var item in Answers[type])
        {
            textLines.Enqueue(item);
        }

        if(optionPanel.activeSelf) optionPanel.SetActive(false);

        if (!dialogueText.gameObject.activeSelf)
        {
            dialogueText.gameObject.SetActive(true);
        }

        StartCoroutine(TypeText(textLines.Dequeue(), () => { if (nextDialogue != null) nextDialogue.SetActive(true); fullText = true; }));
    }

    private async void NextDialogueLine()
    {
        if (nextDialogue.activeSelf && nextDialogue != null) nextDialogue.SetActive(false);
        if (fullText) fullText = false;

        if (trigger.ContainsKey(index))
        {
            StopAllCoroutines();
            await StartCoroutine(TypeText(textLines.Dequeue(), () => { if (nextDialogue != null) nextDialogue.SetActive(true); }));

            /*dialogueText.text = string.Empty;
            dialogueText.gameObject.SetActive(false);

            optionPanel.SetActive(true);

            for (int i = 0; i < buttons.transform.childCount; i++)
            {
                Transform option = buttons.transform.GetChild(i);
                option.GetComponent<UIHelper>().type = NPCS[npcName].GetComponent<ItemAttributes>().Items[option.gameObject.name].Type;
                option.GetComponent<UIHelper>().Text.GetComponent<Text>().text = NPCS[npcName].GetComponent<ItemAttributes>().Items[option.gameObject.name].ItemName;
            }*/

            isAnswer = true;
            didDialogueStart = false;
            trigger = new Dictionary<int, bool>();
            index = 0;
        }
        else if (textLines.Count == 0 && isAnswer)
        {
            StopAllCoroutines();
            dialogueText.text = string.Empty;
            dialogueBox.GetComponent<Animator>().SetTrigger(GameConstants.CLOSE_BOX);
            await new UnityAsync.WaitForSeconds(closeBoxClipDuration);
            dialogueText.gameObject.SetActive(false);
            dialogueBox.SetActive(false);
            isAnswer = false;
            trigger = new Dictionary<int, bool>();
            didDialogueStart = false;


            //Nehuen
            controller.dialogueStart = false;
            return;
        }
        else
        {
            await StartCoroutine(TypeText(textLines.Dequeue(), () => { if (nextDialogue != null) nextDialogue.SetActive(true); fullText = true; }));
        }


    }
    
    private IEnumerator TypeText(string line, Action Done)
    {
        index++;
        dialogueText.text = string.Empty;

        char[] textToShow = line.ToCharArray();

        for (int index = 0; index < textToShow.Length; index++)
        {
            dialogueText.text += textToShow[index];
            //if (index % 3 == 0) SoundManager.instance.PlayAuxiliarEffect(GameConstants.TYPEWRITER_B);
            yield return new UnityEngine.WaitForSecondsRealtime(typingTime);
        }
        //yield return new UnityEngine.WaitUntil(() => SoundManager.instance.AuxiliarEffects.isPlaying);
        //SoundManager.instance.StopAuxiliarEffect();

        Done();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            if (!enter)
            {
                enter = true;
                dialogues = collision.gameObject.GetComponent<Dialogues>()._Dialogues;
                Answers = collision.gameObject.GetComponent<Dialogues>().Dict;
                npcName = collision.gameObject.name;
                exit = false;
            }
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            if (!exit)
            {
                exit = true;
                dialogues = new Dialogue();
                Answers = new Dictionary<AnswerType, List<string>>();
                npcName = string.Empty;
                enter = false;
            }
            isInRange = false;
        }
    }

    public void OnAfterDeserialize()
    {
        NPCS = new Dictionary<string, GameObject>();
        int index = 1;

        foreach (GameObject item in npcList)
        {
            NPCS.Add($"NPC{index}", item);
            index++;
        }
    }
    public void OnBeforeSerialize()
    { }

}
