using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillUpgradeType 
{
    None,

    Dash,
    Dash_CloneOnStart,
    Dash_CloneOnStartAndArrival,
    Dash_ShardOnStart,
    Dash_ShardOnStartAndArrival,

    Shard,
    Shard_MoveToEnemy,
    Shard_MultiCast,
    Shard_Teleport,
    Shard_TeleportHpRewind,

    SwordThrow,
    SwordThrow_Spin,
    SwordThrow_Pierce,
    SwordThrow_Bounce,

    TimeEcho,
    TimeEcho_SingleAttack,
    TimeEcho_MultiAttack,
    TimeEcho_ChanceToDuplicate,

    TimeEcho_HealWisp,

    TimeEcho_CleanseWisp,
    TimeEcho_CooldownWisp,

    Domain_SlowingDown,
    Domain_EchoSpam,
    Domain_ShardSpam

}
