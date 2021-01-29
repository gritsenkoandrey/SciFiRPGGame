using UnityEngine;


public class AimingShotSkill : Skill
{
    [SerializeField] private float _range = 7f;
    [SerializeField] private int _damage = 25;
    [SerializeField] private ParticleSystem _castEffect;
    [SerializeField] private ParticleSystem _aimingShotEffect;

    protected override void Start()
    {
        base.Start();
        _aimingShotEffect.transform.SetParent(null);
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
            _castEffect.Stop();
            _aimingShotEffect.transform.position = enemy.transform.position;
            _aimingShotEffect.transform.rotation = Quaternion.LookRotation(enemy.transform.position - unit.transform.position);
            _aimingShotEffect.Play();
        }
        base.OnCastComplete();
    }

    private void OnDestroy()
    {
        if (isServer) Destroy(_aimingShotEffect.gameObject);
    }
}