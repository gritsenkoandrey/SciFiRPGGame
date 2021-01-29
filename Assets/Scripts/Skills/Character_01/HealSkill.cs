using UnityEngine;


public class HealSkill : Skill
{
    [SerializeField] private int _healAmount = 10;
    [SerializeField] private ParticleSystem _particle = null;

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