using UnityEngine;
using UnityEngine.Networking;


[System.Obsolete]
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
        base.OnStartServer();

        _currentHealth = _maxHealth;
    }

    public void SetHealthRate(float rate)
    {
        _currentHealth = rate == 0 ? 0 : (int)(_maxHealth / rate);
    }
}