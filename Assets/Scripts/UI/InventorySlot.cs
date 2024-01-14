using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    private TextMeshProUGUI ItemAmount;
    private TextMeshProUGUI ItemName;
    private Transform Parent;
    [HideInInspector]
    public bool hasItem = false;
    [HideInInspector]
    public int itemAmount;
    [HideInInspector]
    public ItemType itemType;

    private void Start()
    {
        Parent = GetComponentInParent<Transform>();
        ItemAmount = Parent.Find("Item Amount").GetComponent<TextMeshProUGUI>();
        ItemName = Parent.Find("Item Name").GetComponent<TextMeshProUGUI>();
    }

    public void addItem(ItemType type, int amount)
    {
        if (amount > 0)
        {
            hasItem = true;
            ItemName.text = type.ToString();
            ItemAmount.text = amount.ToString();
        }
        if (amount == 0)
        {
            hasItem = false;
            ItemName.text = string.Empty;
            ItemAmount.text = string.Empty;
        }
        itemAmount = amount;
        itemType = type;
    }
}
