using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class UI_SkillTree : MonoBehaviour,ISaveable
{
    [SerializeField] private  int skillPoints;
    [SerializeField] private TextMeshProUGUI skillPointsText;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;
    private UI_TreeNode[] allTreeNodes;

    public Player_SkillManager skillManager { get; private set; }


    private void Start()
    {
        UpdateAllConnections();
        UpdateSkillPointsUI();
    }

    private void UpdateSkillPointsUI()
    {
        skillPointsText.text = skillPoints.ToString();
    }

    public void UnlockDefaultSkills()
    {
        allTreeNodes = GetComponentsInChildren<UI_TreeNode>(true);
        skillManager = FindAnyObjectByType<Player_SkillManager>();

        foreach (var node in allTreeNodes)
            node.UnlockDefaultSkills();
    }

    [ContextMenu("Reset Skill Tree")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>(true);

        foreach(var node in skillNodes)
        {
            node.Refund();
        }
    }
    public bool EnoughskillPoints(int cost)=>skillPoints >= cost;
    public void RemoveSkillPoints(int cost)
    {
        skillPoints -= cost;
        UpdateSkillPointsUI();
    }
    public void AddSkillPoints(int points)
    {
        skillPoints+= points;
        UpdateSkillPointsUI();
    }


    [ContextMenu("Update All Connections")]

    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }

    public void LoadData(GameData data)
    {
        skillPoints = data.skillPoints;

        foreach(var node in allTreeNodes)
        {
            string skillName = node.skillData.displayName;

            if (data.skillTreeUI.TryGetValue(skillName, out bool unlocked) && unlocked)
                node.UnlockWithSaveData();
        }

        foreach(var skill in skillManager.allSkills)
        {
            if(data.skillUpgrades.TryGetValue(skill.GetSkillType(),out SkillUpgradeType upgradeType))
            {
                var upgradeNode = allTreeNodes.FirstOrDefault(node => node.skillData.upgradeData.upgradeType == upgradeType);

                if (upgradeNode != null)
                    skill.SetSkillUpgrade(upgradeNode.skillData);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.skillPoints = skillPoints;
        data.skillTreeUI.Clear();
        data.skillUpgrades.Clear();

        foreach(var node in allTreeNodes)
        {
            string skillName = node.skillData.displayName;//将节点名称存入字典
            data.skillTreeUI[skillName] = node.isUnlocked;//将技能节点解锁状态存入字典
        }

        foreach(var skill in skillManager.allSkills)
        {
            data.skillUpgrades[skill.GetSkillType()] = skill.GetUpgrade();//技能类型为键，升级类型为值
        }
    }
}
