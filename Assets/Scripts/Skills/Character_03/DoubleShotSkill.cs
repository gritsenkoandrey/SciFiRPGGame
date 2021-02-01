using UnityEngine;


public class DoubleShotSkill : UpgradeableSkill
{
    [SerializeField] private float _range = 7f;
    [SerializeField] private int _damage = 25;
    [SerializeField] private ParticleSystem _doubleShotEffect;

    public override int Level
    {
        set
        {
            base.Level = value;
            _damage = 25 + 3 * Level;
        }
    }

    protected override void Start()
    {
        base.Start();
        _doubleShotEffect.transform.SetParent(null);
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
        else base.OnUse();
    }

    protected override void OnCastComplete()
    {
        Unit enemy = target.GetComponent<Unit>();
        if (isServer)
        {
            if (enemy.HasInteract)
            {
                enemy.TakeDamage(unit.gameObject, _damage);
                unit.SetFocus(enemy);
            }
        }
        else
        {
            _doubleShotEffect.transform.position = enemy.transform.position;
            _doubleShotEffect.transform.rotation = Quaternion.LookRotation(enemy.transform.position - unit.transform.position);
            _doubleShotEffect.Play();
        }
        base.OnCastComplete();
    }

    private void OnDestroy()
    {
        if (isServer) Destroy(_doubleShotEffect.gameObject);
    }
}