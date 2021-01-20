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
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
            }
        }
    }

    public void SetHealthRate(float rate)
    {
        _currentHealth = rate == 0 ? 0 : (int)(maxHealth / rate);
    }
}