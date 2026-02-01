using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAsync;
using System;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject nextDialogue;
    [SerializeField] private GameObject optionPanel;
    private float typingTime = 0.03f;

    private Queue<string> dialogues;

    private DialogueStates dialogueState = DialogueStates.StandBy;

    private bool autoTextComplete;

    private Animator DialogueBoxAnimator;

    private float openBoxClipDuration, closeBoxClipDuration;

    public GameObject NextDialogue { get => nextDialogue; set => nextDialogue = value; }
    public GameObject OptionPanel { get => optionPanel; set => optionPanel = value; }
    internal DialogueStates DialogueState { get => dialogueState; set => dialogueState = value; }
    public bool AutoTextComplete { get => autoTextComplete; set => autoTextComplete = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            dialogues = new Queue<string>();
            if (dialogueBox != null) DialogueBoxAnimator = dialogueBox.GetComponent<Animator>();

            if (DialogueBoxAnimator != null)
            {
                openBoxClipDuration =
                    DialogueBoxAnimator.runtimeAnimatorController.animationClips.Where(x => x.name.Equals(GameConstants.OPEN_BOX)).FirstOrDefault().length;

                closeBoxClipDuration =
                    DialogueBoxAnimator.runtimeAnimatorController.animationClips.Where(x => x.name.Equals(GameConstants.CLOSE_BOX)).FirstOrDefault().length;
            }

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public async void StartDialogue(Dialogue dialogue)
    {
        dialogues.Clear();
        //if (!dialogueState.Equals(DialogueStates.Init)) DialogueState = DialogueStates.Init;

        foreach (var item in dialogue.Lines)
        {
            dialogues.Enqueue(item);
        }

        if (!dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(true);
            await new UnityAsync.WaitForSeconds(openBoxClipDuration);
        }

        SetNextDialogue();
    }

    public void SetNextDialogue()
    {
        if (nextDialogue.activeSelf && nextDialogue != null) nextDialogue.SetActive(false);

        /*if (dialogues.Count == 0)
        {
            StopAllCoroutines();
            if (!DialogueState.Equals(DialogueStates.End)) DialogueState = DialogueStates.End;
            dialogueBox.GetComponentInChildren<Text>().text = string.Empty;
            dialogueBox.GetComponent<Animator>().SetTrigger(GameConstants.CLOSE_BOX);
            return;
        }*/
        StopAllCoroutines();
        StartCoroutine(TypeText(false, dialogues.Dequeue(), () => { if (nextDialogue != null) nextDialogue.SetActive(true); }));

    }

    private IEnumerator TypeText(bool isSequence, string line, Action Done)
    {
        dialogueBox.GetComponentInChildren<Text>().text = string.Empty;

        char[] textToShow = line.ToCharArray();

        for (int index = 0; index < textToShow.Length; index++)
        {
            dialogueBox.GetComponentInChildren<Text>().text += textToShow[index];
            //if (index % 3 == 0) SoundManager.instance.PlayAuxiliarEffect(GameConstants.TYPEWRITER_B);
            yield return new UnityEngine.WaitForSecondsRealtime(typingTime);
        }
        //yield return new UnityEngine.WaitUntil(() => SoundManager.instance.AuxiliarEffects.isPlaying);
        //SoundManager.instance.StopAuxiliarEffect();

        if (isSequence) yield return new UnityEngine.WaitForSeconds(2f);
        Done();
    }
}