using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class UnitMotor : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _target;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_target != null)
        {
            if (_agent.velocity.magnitude == 0)
            {
                FaceTarget();
            }
            _agent.SetDestination(_target.position);
        }
    }

    public void MoveToPoint(Vector3 point)
    {
        _agent.SetDestination(point);
    }

    private void FaceTarget()
    {
        Vector3 direction = _target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0.0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
    }

    public void FollowTarget(Interactable newTarget)
    {
        _agent.stoppingDistance = newTarget.radius;
        _target = newTarget.interactionTransform;
    }

    public void StopFollowingTarget()
    {
        _agent.stoppingDistance = 0.0f;
        _agent.ResetPath();
        _target = null;
    }
}