using UnityEngine;

public class ItemAttributes : MonoBehaviour
{
    [SerializeField] private AnswerType type;

    public AnswerType Type { get => type; set => type = value; }
}
