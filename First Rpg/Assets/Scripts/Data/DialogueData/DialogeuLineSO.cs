using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Dialogue Data/New Line Data", fileName = "Line - ")]
public class DialogeuLineSO : ScriptableObject
{
    [Header("Dialogue info")]
    public string dialoguiGroupName;
    public DialogueSpeakerSO speaker;

    [Header("Text options")]
    [TextArea] public string[] textLine;

    [Header("Choices info")]
    [TextArea] public string playerChoiceAnswer;
    public DialogeuLineSO[] choiceLines;

    [Header("Dialogue Action")]
    [TextArea] public string actionLine;
    public DialogueActionType actionType;


    public string GetFirstLine() => textLine[0];

    public string GetRandomLine()
    {
        return textLine[Random.Range(0, textLine.Length)];
    }
}
