﻿using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(UnitStats))]
public class Combat : NetworkBehaviour
{
    [SerializeField] private float _attackSpeed = 1.0f;

    public float attackDistance = 0.0f;

    private UnitStats _unitStats;
    private float _attackCooldown = 0.0f;

    public delegate void CombatDelegate();
    [SyncEvent] public event CombatDelegate EventOnAttack;

    private void Start()
    {
        _unitStats = GetComponent<UnitStats>();
    }

    private void Update()
    {
        if (_attackCooldown > 0)
        {
            _attackCooldown -= Time.deltaTime;
        }
    }

    public bool Attack(UnitStats targetStats)
    {
        if (_attackCooldown <= 0)
        {
            targetStats.TakeDamage(_unitStats.damage.GetValue());
            EventOnAttack();
            _attackCooldown = 1.0f / _attackSpeed;
            return true;
        }
        return false;
    }
}