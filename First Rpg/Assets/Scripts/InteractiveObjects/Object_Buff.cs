using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Object_Buff : MonoBehaviour
{
    private Player_Stats statsToModify;

    [Header("Buff details")]
    [SerializeField] private BuffEffectData[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 4;


    [Header("Floaty movement")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float yoffset = Mathf.Sin(Time.time * floatSpeed)*floatRange;
        transform.position = startPosition+new Vector3(0, yoffset);  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        statsToModify = collision.GetComponent<Player_Stats>();

        if (statsToModify.CanApplyBuffof(buffName))
        {
            statsToModify.ApplyBuff(buffs, buffDuration, buffName);
            Destroy(gameObject);
        }
    }
}
