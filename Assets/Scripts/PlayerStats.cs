using UnityEngine;
using UnityEngine.Networking;


[System.Obsolete]
public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private int _maxHealth = 0;
    [SyncVar] private int _currentHealth;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        _currentHealth = _maxHealth;
    }
}