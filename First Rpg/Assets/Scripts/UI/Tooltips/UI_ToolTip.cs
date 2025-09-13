using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private Vector2 offset= new Vector2(300,20);

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();   
    }

    public virtual void ShowToolTip(bool show, RectTransform taretRect)
    {
        if (show == false)
        {
            if (rect == null)
                return;
            rect.position = new Vector2(0, 9999);
            return;
        }

        UpdatePosition(taretRect);

    }

    private void UpdatePosition(RectTransform targetRect)
    {
        float screenCenterX = Screen.width / 2; 
        float screenTop = Screen.height;
        float screenBottom = 0f;

        Vector2 targetPosition = targetRect.position;

        targetPosition.x = targetPosition.x>screenCenterX? targetPosition.x-offset.x:targetPosition.x+offset.x;

        rect.position = targetPosition;

        float veritcalHalf = rect.sizeDelta.y / 2;
        float topY= targetPosition.y+veritcalHalf;
        float bottomY = targetPosition.y- veritcalHalf;

        if(topY>screenTop)
            targetPosition.y= screenTop - veritcalHalf - offset.y;
        else if(bottomY<screenBottom)
            targetPosition.y= screenBottom + veritcalHalf +offset.y;


        rect.position = targetPosition; 

        
    }

    protected string GetColoredText(string color, string text)
    {
        return $"<color={color}>-{text}</color>";
    }
}
