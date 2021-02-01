using UnityEngine;


public class HideSkill : UpgradeableSkill
{
    [SerializeField] private ParticleSystem _hideEffect = null;

    public override int Level
    {
        set
        {
            base.Level = value;
            castTime = Level < 10 ? 10 - Level : 1;
        }
    }

    protected override void OnUse()
    {
        if (isServer)
        {
            unit.RemoveFocus();
            unit.HasInteract = false;
        }
        else _hideEffect.Play();
        base.OnUse();
    }

    protected override void OnCastComplete()
    {
        if (isServer)
        {
            unit.HasInteract = true;
        }
        else _hideEffect.Stop();
        base.OnCastComplete();
    }
}