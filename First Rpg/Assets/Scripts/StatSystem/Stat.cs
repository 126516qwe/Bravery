using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers;

    private bool needToCalculate=true;
    private float finalValue;

    public float GetValue()
    {
        if (needToCalculate)
        {
            finalValue = GetFinalValue();
            needToCalculate = false;
        }
            return finalValue;
    }

    public void AddModifier(float value,string source)
    {
        StatModifier modToAdd = new StatModifier(value,source);
        modifiers.Add(modToAdd);
        needToCalculate = true;
    }

    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(modifiers=>modifiers.source == source); //�൱�ڱ������б�
        needToCalculate = true;
    }//���б���Ѱ����Ϊsource����Ȼ���Ƴ�


    private float GetFinalValue()
    {
        float finalValue = baseValue;

        foreach (var modifier in modifiers)
        {
            finalValue = finalValue + modifier.value;
        }
        return finalValue;  
    }

    public void SetBaseValue(float value)=>baseValue = value;   
}
[Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }//����ʱ���ٳ�ʼ������ı���
}