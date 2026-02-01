using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityAsync;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

public class DialogueSystem : MonoBehaviour, ISerializationCallbackReceiver
{
    private bool isInRange, didDialogueStart, fullText, trigger, isAnswer;

    public bool optionSelected;

    [SerializeField] private Dialogue dialogues;

    public AnswerType type;

    private Queue<string> textLines;

    [SerializeField] private List<Answer> answers;

    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject nextDialogue;
    [SerializeField] private GameObject optionPanel;


    private Animator DialogueBoxAnimator;
    private float typingTime = 0.03f;

    private float openBoxClipDuration, closeBoxClipDuration;

    [SerializeField] Dictionary<AnswerType, List<string>> Answers;

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
    }

    // Update is called once per frame
    void Update()
    {
        float fire1 = Input.GetAxis("Fire1");

        Debug.Log(fire1);
        Debug.Log(isInRange);

        if ( isInRange && fire1 > 0 && !isAnswer)// Agregar is in range 
        {
            if (!didDialogueStart)
            {
                StartDialogue(); 
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

        if (isAnswer && fire1 > 0 && fullText)
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
                trigger = item.TriggerEvent;
            }
        }

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

        if (trigger)
        {
            StopAllCoroutines();
            await StartCoroutine(TypeText(textLines.Dequeue(), () => { if (nextDialogue != null) nextDialogue.SetActive(true); fullText = true; }));

            Debug.Log("Inicia seleccion de producto");
           /* dialogueText.text = string.Empty;
            dialogueText.gameObject.SetActive(false);

            optionPanel.SetActive(true);*/

            isAnswer = true;
            trigger = didDialogueStart = false;
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
            trigger = didDialogueStart = false;
            return;
        }

    }
    
    private IEnumerator TypeText(string line, Action Done)
    {
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
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            isInRange = false;
        }
    }

    public void OnAfterDeserialize()
    {
        Answers = new Dictionary<AnswerType, List<string>>();

        foreach (Answer item in answers)
        {
            Answers.Add(item.Type, item.TextLines);
        }
    }

    public void OnBeforeSerialize()
    { }

}
