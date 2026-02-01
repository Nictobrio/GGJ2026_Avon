using UnityEngine;
using System.Collections.Generic;

public class Dialogues : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] private Dialogue dialogues;

    [SerializeField] private List<Answer> answers;

    [SerializeField] Dictionary<AnswerType, List<string>> Answers;

    public Dialogue _Dialogues { get => dialogues; set => dialogues = value; }
    public List<Answer> Answers1 { get => answers; set => answers = value; }
    public Dictionary<AnswerType, List<string>> Dict { get => Answers; set => Answers = value; }

    public void OnAfterDeserialize()
    {
        Dict = new Dictionary<AnswerType, List<string>>();

        foreach (Answer item in Answers1)
        {
            Dict.Add(item.Type, item.TextLines);
        }
    }

    public void OnBeforeSerialize()
    { }
}
