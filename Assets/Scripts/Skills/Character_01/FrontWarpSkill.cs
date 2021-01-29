using UnityEngine;


public class FrontWarpSkill : Skill
{
    [SerializeField] private float _warpDistance = 7.0f;

    protected override void OnUse()
    {
        if (isServer)
        {
            unit.Motor.StopFollowingTarget();
        }
        base.OnUse();
    }

    protected override void OnCastComplete()
    {
        if (isServer)
        {
            unit.transform.Translate(Vector3.forward * _warpDistance);
            unit.Motor.StopFollowingTarget();
        }
        base.OnCastComplete();
    }
}