using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CraftListButton : MonoBehaviour
{
    [SerializeField] private ItemListDataSO craftData;
    private UI_CraftSlot[] craftSlots;

    public void SetCraftSlots(UI_CraftSlot[] craftSlots)=> this.craftSlots = craftSlots;    

    public void UpdateCraftSlots()
    {
        if(craftSlots == null)
        {
            Debug.Log("you need to assign craft list datal");
            return;
        }

        foreach(var slot in craftSlots)
        {
            slot.gameObject.SetActive(false);
        }

        for(int i = 0; i< craftData.itemList.Length; i++)
        {
            Item_DataSO itemData = craftData.itemList[i];

            craftSlots[i].gameObject.SetActive(true);
            craftSlots[i].SetupButton(itemData);
        }
    }
}
