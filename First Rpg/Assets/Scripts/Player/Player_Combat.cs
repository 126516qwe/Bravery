using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [SerializeField] private float counterRecovery=0.01f;
    [SerializeField] private LayerMask whatIsCounterable;
    public bool CounterAttackPerformed()
    {
        bool hasPerformCounter=false;
        
        foreach (var target in GetDetectedColliders(whatIsCounterable))
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            
            if (counterable ==null)
            {
               continue;
            }
            if (counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                hasPerformCounter = true;
            }
            
        }
        
        return hasPerformCounter;
    }
    
    public float GetCounterRecoveryDuration()=>counterRecovery;
}
