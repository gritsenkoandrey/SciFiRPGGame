﻿using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class Unit : Interactable
{
    [SerializeField] protected UnitMotor _unitMotor;
    [SerializeField] protected UnitStats _unitStats;

    protected Interactable focus;
    protected bool isDead;

    public delegate void UnitDelegate();
    [SyncEvent] public event UnitDelegate EventOnDamage;
    [SyncEvent] public event UnitDelegate EventOnDie;
    [SyncEvent] public event UnitDelegate EventOnRevive;

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

    public override bool Interact(GameObject user)
    {
        Combat combat = user.GetComponent<Combat>();
        if (combat != null)
        {
            if (combat.Attack(_unitStats))
            {
                EventOnDamage();
                return true;
            }
        }
        return base.Interact(user);
    }

    protected virtual void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            focus = newFocus;
            _unitMotor.FollowTarget(newFocus);
        }
    }

    protected virtual void RemoveFocus()
    {
        focus = null;
        _unitMotor.StopFollowingTarget();
    }

    [ClientCallback]
    protected virtual void Die()
    {
        isDead = true;
        if (isServer)
        {
            HasInteract = false;
            RemoveFocus();
            _unitMotor.MoveToPoint(transform.position);
            EventOnDie();
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
            HasInteract = true;
            _unitStats.SetHealthRate(1.0f);
            EventOnRevive();
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