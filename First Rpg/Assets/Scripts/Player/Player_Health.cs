using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : Entity_Health,ISaveable
{
    private Player player;
    private float reliveHP = 100f;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            Die();
    }
    protected override void Die()
    {
        base.Die();


        player.ui.OpenDeathScreenUI();
        GameManager.instance.isDefualLoad = true;
        //GameManager.instance.SetLastPlayerPosition(transform.position);
        //GameManager.instance.RestartScene();
    }

    public void LoadData(GameData data)
    {
        if (GameManager.instance.isDefualLoad)
        {
            currentHealth = reliveHP;
            GameManager.instance.isDefualLoad = false;
            return;
        }
        currentHealth = data.currentHP;
    }

    public void SaveData(ref GameData data)
    {
        data.currentHP = currentHealth;
    }
}
