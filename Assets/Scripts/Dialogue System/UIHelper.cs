using UnityEngine;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{
    [SerializeField] private GameObject dialogueSystem;
    [SerializeField] private GameObject text;
    public AnswerType type = AnswerType.Default;

    public DialogueSystem DialogueSystem;
    public GameObject Text { get => text; set => text = value; }

   /* private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        Cursor.lockState -= CursorLockMode.Locked;
    }*/

    public void accionButton()
    {
        DialogueSystem.type = type;
        DialogueSystem.optionSelected = true;

        Debug.LogWarning("Sucedió");
        
    }
}
