using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueDatabase", menuName = "GGJ/Database/Dialogue/New Database")]
public class DialogueDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private List<Dialogue> dialogues;

    Dictionary<DialogueStep, Dialogue> dialogueDict;

    public Dictionary<DialogueStep, Dialogue> DialogueDict { get => dialogueDict; set => dialogueDict = value; }

    public void OnAfterDeserialize()
    {
        dialogueDict = new Dictionary<DialogueStep, Dialogue>();

        foreach(Dialogue dialogue in dialogues)
        {
                dialogueDict.Add(dialogue.Step, dialogue);
        }
    }

    public void OnBeforeSerialize() {}
}
