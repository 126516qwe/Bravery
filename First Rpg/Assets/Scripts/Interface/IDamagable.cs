using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable { 
    public bool TakeDamage(float damage,float elementalDamage,ElementType element,Transform damageDealer);
}
