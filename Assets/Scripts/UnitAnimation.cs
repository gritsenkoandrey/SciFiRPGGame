using UnityEngine;
using UnityEngine.AI;


public class UnitAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;

    private static readonly int _moving = Animator.StringToHash("Moving");

    private void FixedUpdate()
    {
        Moving();
    }

    private void Moving()
    {
        _animator.SetBool(_moving, _agent.hasPath);
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