using UnityEngine;


public class EnemyStats : UnitStats
{
    public override void OnStartServer()
    {
        CurrentHealth = maxHealth;
    }
}