using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] private DialogueStep step;

    [SerializeField]
    [TextArea(3, 10)]
    private List<string> lines;

    public List<string> Lines { get => lines; set => lines = value; }
    public DialogueStep Step { get => step; set => step = value; }

    [System.Serializable]
    public class Option
    {
        [SerializeField]
        private bool optionSelected;

        [SerializeField]
        [TextArea(3, 10)]
        private List<string> optionList;

        public bool OptionSelected { get => optionSelected; set => optionSelected = value; }
        public List<string> OptionList { get => optionList; set => optionList = value; }
    }
}