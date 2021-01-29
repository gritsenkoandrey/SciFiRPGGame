using UnityEngine;


public class HideSkill : Skill
{
    [SerializeField] private ParticleSystem _hideEffect;

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