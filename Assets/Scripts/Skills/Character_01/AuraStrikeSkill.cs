using UnityEngine;


public class AuraStrikeSkill : Skill
{
    [SerializeField] private int _damage = 0;
    [SerializeField] private float _radius = 0.0f;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private ParticleSystem _auraEffect = null;

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
            Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _enemyMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                Unit enemy = colliders[i].GetComponent<Unit>();
                if (enemy != null && enemy.HasInteract)
                {
                    enemy.TakeDamage(unit.gameObject, _damage);
                }
            }
        }
        else
        {
            _auraEffect.Play();
        }
        base.OnCastComplete();
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}