using System.Collections.Generic;
using UnityEngine;

public class ItemAttributes : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] Dictionary<string, Item> items;

    [SerializeField] List<Item> itemList;

    public Dictionary<string, Item> Items { get => items; set => items = value; }

    [System.Serializable]
    public class Item
    {
        [SerializeField] private AnswerType type;

        [SerializeField]
        [TextArea(3, 10)]
        private string itemName;
        public AnswerType Type { get => type; set => type = value; }

        public string ItemName { get => itemName; set => itemName = value; }
    }

    public void OnAfterDeserialize()
    {
        Items = new Dictionary<string, Item>();
        int index = 0;

        foreach (Item item in itemList)
        {
            Items.Add($"Option{index}", item);
            index++;
        }
    }
    public void OnBeforeSerialize()
    { }

}
