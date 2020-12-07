using UnityEngine;


public class UnitTriggerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Unit _unit;
    [SerializeField] private Combat _combat;

    private void Start()
    {
        _unit.EventOnDamage += Damage;
        _unit.EventOnDie += Die;
        _unit.EventOnRevive += Revive;
        _combat.EventOnAttack += Attack;
    }

    private void Damage()
    {
        _animator.SetTrigger("Damage");
    }

    private void Die()
    {
        _animator.SetTrigger("Die");
    }

    private void Revive()
    {
        _animator.ResetTrigger("Damage");
        _animator.ResetTrigger("Attack");
        _animator.SetTrigger("Revive");
    }

    private void Attack()
    {
        if (Random.Range(0, 2) == 1)
        {
            _animator.SetTrigger("Attack");
        }
        else
        {
            _animator.SetTrigger("NextAttack");
        }
    }
}