using System.Collections.Generic;
using UnityEngine;
using static Dialogue;

[System.Serializable]
public class Dialogue
{
    [SerializeField] private List<Line> lines;

    public List<Line> Lines { get => lines; set => lines = value; }

    [System.Serializable]
    public class Line
    {
        [SerializeField]
        private bool triggerEvent;

        [SerializeField]
        [TextArea(3, 10)]
        private string textLine;

        public bool TriggerEvent { get => triggerEvent; set => triggerEvent = value; }
        public string TextLine { get => textLine; set => textLine = value; }
    }
}

[System.Serializable]
public class Answer
{
    [SerializeField] private AnswerType type;

    [SerializeField]
    [TextArea(3, 10)]
    private List<string> textLines;

    public List<string> TextLines { get => textLines; set => textLines = value; }
    public AnswerType Type{ get => type; set => type = value; }
}