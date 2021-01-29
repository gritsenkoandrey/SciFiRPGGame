using UnityEngine;
using UnityEngine.Networking;


public class UnitStats : NetworkBehaviour
{
    [SerializeField] protected int maxHealth = 0;
    [SyncVar] private int _currentHealth;

    public Stat damage;
    public Stat armor;
    public Stat moveSpeed;

    public virtual int CurrentHealth
    {
        get { return _currentHealth; }
        protected set { _currentHealth = value; }
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= armor.GetValue();
        if (damage > 0)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
            }
        }
    }

    public void SetHealthRate(float rate)
    {
        CurrentHealth = rate == 0 ? 0 : (int)(maxHealth / rate);
    }

    public void AddHealth(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > maxHealth)
        {
            CurrentHealth = maxHealth;
        }
    }
}