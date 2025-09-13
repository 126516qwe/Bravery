using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    private UI_TreeConnectHandler connectHandler;

    [Header("Unlock details")]
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isLocked;

    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    [SerializeField] private string lockedColorHex = "#5C5C5C";
    private Color lastColor;

    private void Start()
    {
        if(isUnlocked == false)
            UpdateIconColor(GetColorByHex(lockedColorHex));

        UnlockDefaultSkills();
    }

    public  void UnlockDefaultSkills()
    {
        GetNeededComponents();

        if (skillData.unlockedByDefault)
            Unlock();
    }
    private void GetNeededComponents()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>(true);
        connectHandler = GetComponent<UI_TreeConnectHandler>();
    }

    public void Refund()
    {
        if (skillData.unlockedByDefault)
            return;

        if (isUnlocked == true)
            skillTree.AddSkillPoints(skillData.cost);

        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));

        connectHandler.UnlockConnectionImage(true);

        
            
        
    }//重置技能点
    private void Unlock()
    {
        if (isUnlocked)
        {
            Debug.Log("Skill is already unlocked!");
            return;
        }

        isUnlocked = true;
        UpdateIconColor(Color.white);
        skillTree.RemoveSkillPoints(skillData.cost);

        LockConflictNodes();
        connectHandler.UnlockConnectionImage(true);

        skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData);
    }

    public void UnlockWithSaveData()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        skillTree.RemoveSkillPoints(skillData.cost);

        connectHandler.UnlockConnectionImage(true);
    }

    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
            return false;

        if (skillTree.EnoughskillPoints(skillData.cost) == false)
            return false;

        foreach (var node in neededNodes)//要求的节点没解锁就不能解锁
        {
            if (node.isUnlocked == false)
                return false;
        }

        foreach (var node in conflictNodes)//矛盾的节点解锁了就不能解锁
        {
            if (node.isUnlocked)
            {
                return false;
            }
        }

        return true;
    }


    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes) 
        { 
            node.isLocked = true;
            node.LockChildNodes();
        }
    }

    public void LockChildNodes()
    {
        isLocked = true;

        foreach(var node in connectHandler.GetChildNodes())
            node.LockChildNodes();
    }


    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;

        lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            Unlock();
        else if (isLocked)
            ui.skillToolTip.LockedSkillEffect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect,skillData, this);

        if (isUnlocked || isLocked)
            return;

        ToggleNodeHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, rect);
        ui.skillToolTip.StopLockedSkillEffect();

        if (isUnlocked || isLocked)
            return;

        ToggleNodeHighlight(false);
    }

    private void ToggleNodeHighlight(bool highlight)
    {
        Color highlighColor = Color.white * .9f; highlighColor.a = 1;
        Color colorToApply = highlight ? highlighColor : lastColor;
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);

        return color;
    }

    private void OnDisable()
    {
        if (isLocked)
            UpdateIconColor(GetColorByHex(lockedColorHex));

        if (isUnlocked)
            UpdateIconColor(Color.white);
    }
    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost;
        gameObject.name = "UI_TreeNode - " + skillData.displayName;
    }
}
