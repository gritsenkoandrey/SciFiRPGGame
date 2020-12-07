using UnityEngine;
using UnityEngine.AI;


public class UnitAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;

    private static readonly int _moving = Animator.StringToHash("Moving");

    private void Update()
    {
        Moving();
    }

    private void Moving()
    {
        if (_agent.velocity.magnitude == 0)
        {
            _animator.SetBool(_moving, false);
        }
        else
        {
            _animator.SetBool(_moving, true);
        }
        //_animator.SetBool(_moving, _agent.hasPath);
    }

    private void Hit()
    {

    }
    private void FootR()
    {

    }
    private void FootL()
    {

    }
}