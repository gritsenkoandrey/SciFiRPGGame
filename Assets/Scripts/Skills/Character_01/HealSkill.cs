﻿using UnityEngine;


public class HealSkill : UpgradeableSkill
{
    [SerializeField] private int _healAmount = 10;
    [SerializeField] private ParticleSystem _particle = null;

    public override int Level
    {
        set
        {
            base.Level = value;
            _healAmount = 10 + Level;
        }
    }

    protected override void OnCastComplete()
    {
        if (isServer)
        {
            unit.Stats.AddHealth(_healAmount);
        }
        else
        {
            _particle.Play();
        }
        base.OnCastComplete();
    }
}