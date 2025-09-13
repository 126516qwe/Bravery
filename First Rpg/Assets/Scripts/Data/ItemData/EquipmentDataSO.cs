using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Equipment item", fileName = "Equipment data -")]
public class EquipmentDataSO : Item_DataSO 
{
    [Header("Item modifiers")]
    public ItemModifier[] modifiers;
}

[Serializable]
public class ItemModifier
{
    public StatType statType;
    public float value;
}
