using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class UI_QuickItemSlotOption : UI_ItemSlot
{
    private UI_QuickItemSlot currentQuickItemSlot;
 

    public void SetupOption(UI_QuickItemSlot currentQuickItemSlot,Inventory_Item itemToSet)
    {
        this.currentQuickItemSlot = currentQuickItemSlot;
        UpdateSlot(itemToSet);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        currentQuickItemSlot.SetupQuickSlotItem(itemInSlot);
        ui.inGameUI.HideQuickItemOptions();
    }
}
