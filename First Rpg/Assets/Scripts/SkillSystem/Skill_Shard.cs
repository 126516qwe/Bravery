using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard currentShard;
    private Entity_Health playerHealth;

    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 2;

    [Header("Moving Shard Upgrade")]
    [SerializeField] private float shardSpeed = 7;

    [Header("Multicast Shard Upgrade")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges;
    [SerializeField] private bool isReCharging;

    [Header("Teleport Shard Upgrade")]
    [SerializeField] private float shardExistDutation = 10;

    [Header("Health Rewind Shard Upgrade")]
    [SerializeField] private float savedHealthPercent;

    protected override void Awake()
    {
         base.Awake();
        currentCharges = maxCharges;
        playerHealth = GetComponentInParent<Entity_Health>();
    }
    public void CreateShard()
    {
        float detonateTime = GetDetonateTime();

        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(this);//对新创建的shard的SkillObject_Shard的脚本里的数据进行初始化


        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            currentShard.OnExplode += ForceCooldown;
    }

    public void CreatRawShard(Transform target = null,bool shardsCanMove=false)
    {
        bool canMove = shardsCanMove != false?  shardsCanMove: Unlocked(SkillUpgradeType.Shard_MoveToEnemy) || Unlocked(SkillUpgradeType.Shard_MultiCast);

        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);

        shard.GetComponent<SkillObject_Shard>().SetUpShard(this, detonateTime, canMove, shardSpeed,target);
    }

    public void CreateDomainShard(Transform target) 
    { 
    }
    public override void TryUseSkill()
    {
        if(CanUseSkill()==false)
            return;

        if(upgradeType == SkillUpgradeType.None)
            return;

        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();

        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();

        if(Unlocked(SkillUpgradeType.Shard_MultiCast))
            HandleShardMulticast();

        if(Unlocked(SkillUpgradeType.Shard_Teleport))
            HandleShardTeleport();

        if(Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            HandleShardHealthRewind();
    }

    private void HandleShardHealthRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthPercent = playerHealth.GetHeakthPercent();
        }
        else
        {
            SwapPlayerAndShard();
            playerHealth.SetHealthToPercent(savedHealthPercent);
            SetSkillOnCooldown();
        }
    }

    private void HandleShardTeleport()
    {
        if (currentShard == null)
        {
            CreateShard();
        }
        else
        {
            SwapPlayerAndShard();
            SetSkillOnCooldown();
        }
    }

    private void SwapPlayerAndShard()
    {
        Vector3 shardPosition = currentShard.transform.position;
        Vector3 playerPosition = player.transform.position;

        currentShard.transform.position = playerPosition;
        currentShard.Explode();
        player.TeleportPlayer(shardPosition);
    }

    private void HandleShardMulticast()
    {
        if(currentCharges<=0)
            return;

        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharges--;

        if (isReCharging == false)
            StartCoroutine(ShardRechargeCo());
    }

    private IEnumerator ShardRechargeCo()
    {
        isReCharging = true;

        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            currentCharges++;
        }
        isReCharging = false;
    }

    private void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        SetSkillOnCooldown();
    }

    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }
 
   

    public float GetDetonateTime()
    {
        if(Unlocked(SkillUpgradeType.Shard_Teleport)||Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            return shardExistDutation;

        return detonateTime;
    }

    private void ForceCooldown()
    {
        if (OnCooldown() == false)
        {
            SetSkillOnCooldown();
            currentShard.OnExplode -= ForceCooldown;
        }

    }
}
