using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreviwSlot : MonoBehaviour
{
    [SerializeField] private Image materialIcon;
    [SerializeField] private TextMeshProUGUI materialNameValue;

    public void SetupMaterialSlot(Item_DataSO itemData, int avaliableAmount , int requiredAmount)
    {
        materialIcon.sprite = itemData.itemIcon;
        materialNameValue.text = itemData.itemName + " - " + avaliableAmount + "/" + requiredAmount;
    }
}
