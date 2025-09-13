using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory_Merchant : Inventory_Base
{
    private Inventory_Player inventory;

    [SerializeField] private ItemListDataSO shopData;
    [SerializeField] private int minItemsAmount = 4;

    protected override void Awake()
    {
         base.Awake();
        FillShopList();
    }

    public void TryBuyItem(Inventory_Item itemToBuy,bool buyFullStack)
    {
        int amountTobuy = buyFullStack ? itemToBuy.stackSize : 1;

        for(int i = 0; i < amountTobuy; i++)
        {
            if (inventory.gold < itemToBuy.buyPrice)
            {
                Debug.Log("not enough money1");
                return;
            }

            if (itemToBuy.itemData.itemType == ItemType.Material)
            {
                inventory.storage.AddMaterialToStash(itemToBuy);
            }
            else
            {
                if (inventory.CanAddItem(itemToBuy))
                {
                    var itemToadd = new Inventory_Item(itemToBuy.itemData);
                    inventory.AddItem(itemToadd);
                }
            }

            inventory.gold=inventory.gold - itemToBuy.buyPrice;
            RemoveOneItem(itemToBuy);
        }
        TriggerUpDateUI();
        
    }

    public void TrySellItem(Inventory_Item itemToSell,bool sellFullstack)
    {
        int amountToSell = sellFullstack ? itemToSell.stackSize : 1;

        for(int i = 0; i < amountToSell; i++)
        {
            int sellPrice = Mathf.FloorToInt(itemToSell.sellPrice);

            inventory.gold = inventory.gold + sellPrice;
            inventory.RemoveOneItem(itemToSell);
        }
        TriggerUpDateUI();
    }

    public void FillShopList()
    {
        itemList.Clear();
        List<Inventory_Item> possibleItems = new List<Inventory_Item>();

        foreach(var itemData in shopData.itemList)
        {
            int randomziedStack = UnityEngine.Random.Range(itemData.minStackSizeAtShop, itemData.maxStackSizeAtShop + 1);
            int finalStack = Mathf.Clamp(randomziedStack, 1, itemData.maxStackSize);

            Inventory_Item itemToAdd = new Inventory_Item(itemData);
            itemToAdd.stackSize = finalStack;

            possibleItems.Add(itemToAdd);
        }
        int randomItemAmount = UnityEngine.Random.Range(minItemsAmount,maxInventorySize+ 1);
        int finalAmount = Mathf.Clamp(randomItemAmount, 1, possibleItems.Count);

        for(int i = 0; i < finalAmount; i++)
        {
            var randomIndex = UnityEngine.Random.Range(0, possibleItems.Count);
            var item = possibleItems[randomIndex];

            if (CanAddItem(item))
            {
                possibleItems.Remove(item);
                AddItem(item);
            }
        }
        TriggerUpDateUI();
    }

    public void SetInventory(Inventory_Player inventory)=> this.inventory = inventory;
}
