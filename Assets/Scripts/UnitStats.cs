using UnityEngine;
using UnityEngine.Networking;


public class UnitStats : NetworkBehaviour
{
    [SerializeField] private int _maxHealth = 0;
    [SyncVar] private int _currentHealth;

    public Stat damage;

    public int CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
    }

    public override void OnStartServer()
    {
        //base.OnStartServer();

        _currentHealth = _maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
        }
    }

    public void SetHealthRate(float rate)
    {
        _currentHealth = rate == 0 ? 0 : (int)(_maxHealth / rate);
    }
}