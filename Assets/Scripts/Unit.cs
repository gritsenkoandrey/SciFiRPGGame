using UnityEngine;
using UnityEngine.Networking;


[System.Obsolete]
public class Unit : NetworkBehaviour
{
    [SerializeField] protected UnitMotor _unitMotor;
    [SerializeField] protected UnitStats _unitStats;

    protected bool isDead;

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnAliveUpdate() { }
    protected virtual void OnDeadUpdate() { }

    protected void OnUpdate()
    {
        if (isServer)
        {
            if (!isDead)
            {
                if (_unitStats.CurrentHealth == 0)
                {
                    Die();
                }
                else
                {
                    OnAliveUpdate();
                }
            }
            else
            {
                OnDeadUpdate();
            }
        }
    }

    [ClientCallback]
    protected virtual void Die()
    {
        isDead = true;
        if (isServer)
        {
            _unitMotor.MoveToPoint(transform.position);
            RpcDie();
        }
    }

    // [ClientRpc] команда, которая выполняется на клиенте
    [ClientRpc]
    private void RpcDie()
    {
        if (!isServer)
        {
            Die();
        }
    }

    [ClientCallback]
    protected virtual void Revive()
    {
        isDead = false;
        if (isServer)
        {
            _unitStats.SetHealthRate(1.0f);
            RpcReviev();
        }
    }

    [ClientRpc]
    private void RpcReviev()
    {
        if (!isServer)
        {
            Revive();
        }
    }
}