using UnityEngine;


public class ElectroShieldSkill : UpgradeableSkill
{
    [SerializeField] private int _damage;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private ParticleSystem _electroEffect;

    public override int Level
    {
        set
        {
            base.Level = value;
            _damage = 25 + 5 * Level;
        }
    }

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
                if (enemy != null && enemy.HasInteract) enemy.TakeDamage(unit.gameObject, _damage);
            }
        }
        else
        {
            _electroEffect.Play();
        }
        base.OnCastComplete();
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}