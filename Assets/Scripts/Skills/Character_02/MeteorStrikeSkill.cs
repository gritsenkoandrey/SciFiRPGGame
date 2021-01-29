using UnityEngine;


public class MeteorStrikeSkill : Skill
{
    [SerializeField] private float _range = 7f;
    [SerializeField] private float _radius = 3f;
    [SerializeField] private int _damage = 25;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private ParticleSystem _castEffect;
    [SerializeField] private ParticleSystem _meteorStrikeEffect;

    protected override void Start()
    {
        base.Start();
        _meteorStrikeEffect.transform.SetParent(null);
    }

    protected override void OnUse()
    {
        if (isServer)
        {
            if (target != null && target.GetComponent<Unit>() != null)
            {
                if (Vector3.Distance(target.transform.position, unit.transform.position) <= _range)
                {
                    unit.RemoveFocus();
                    base.OnUse();
                }
            }
        }
        else
        {
            _castEffect.Play();
            base.OnUse();
        }
    }

    protected override void OnCastComplete()
    {
        if (isServer)
        {
            Collider[] colliders = Physics.OverlapSphere(target.transform.position, _radius, _enemyMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                Unit enemy = colliders[i].GetComponent<Unit>();
                if (enemy != null && enemy.HasInteract) enemy.TakeDamage(unit.gameObject, _damage);
            }
        }
        else
        {
            _castEffect.Stop();
            _meteorStrikeEffect.transform.position = target.transform.position;
            _meteorStrikeEffect.transform.rotation = Quaternion.LookRotation(target.transform.position - unit.transform.position);
            _meteorStrikeEffect.Play();
        }
        base.OnCastComplete();
    }

    private void OnDestroy()
    {
        if (isServer)
        {
            Destroy(_meteorStrikeEffect.gameObject);
        }
    }
}