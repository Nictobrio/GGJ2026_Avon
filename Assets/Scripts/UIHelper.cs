using UnityEngine;

public class UIHelper : MonoBehaviour
{
    [SerializeField] private GameObject pivotSelector;
    [SerializeField] private GameObject text;

    public GameObject PivotSelector { get => pivotSelector; set => pivotSelector = value; }
    public GameObject Text { get => text; set => text = value; }
}
