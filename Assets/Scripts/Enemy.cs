using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(UnitMotor), typeof(EnemyStats))]
public class Enemy : Unit
{
    [Header("Movement")]
    [SerializeField] private float _moveRadius = 10.0f;
    [SerializeField] private float _minMoveDelay = 4.0f;
    [SerializeField] private float _maxMoveDelay = 12.0f;
    private Vector3 _startPosition;
    private Vector3 _currentDistanation;
    private float _changePosTime;

    [Header("Behaviour")]
    [SerializeField] private bool _agressive = true;
    [SerializeField] private float _rewardExp = 0.0f;
    [SerializeField] private float _viewDistance = 8.0f;
    [SerializeField] private float _agroDistance = 5.0f;
    [SerializeField] private float _revievDelay = 5.0f;

    private float _revievTime;
    private List<Character> _enemies = new List<Character>();

    private void Start()
    {
        _startPosition = transform.position;
        _changePosTime = Random.Range(_minMoveDelay, _maxMoveDelay);
        _revievTime = _revievDelay;
    }

    private void Update()
    {
        OnUpdate();
    }

    protected override void OnDeadUpdate()
    {
        base.OnDeadUpdate();

        if (_revievTime > 0)
        {
            _revievTime -= Time.deltaTime;
        }
        else
        {
            _revievTime = _revievDelay;
            Revive();
        }
    }

    protected override void OnAliveUpdate()
    {
        base.OnAliveUpdate();

        if (focus == null)
        {
            Wandering(Time.deltaTime);

            if (_agressive)
            {
                FindEnemy();
            }
        }
        else
        {
            float distance = Vector3.Distance(focus.interactionTransform.position, transform.position);

            if (distance > _viewDistance || !focus.HasInteract)
            {
                RemoveFocus();
            }
            else if (distance <= interactDistance)
            {
                if (!focus.Interact(gameObject))
                {
                    RemoveFocus();
                }
            }
        }
    }

    //public override bool Interact(GameObject user)
    //{
    //    if (base.Interact(user))
    //    {
    //        SetFocus(user.GetComponent<Interactable>());
    //        return true;
    //    }
    //    return false;
    //}

    private void FindEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _agroDistance, 1 << LayerMask.NameToLayer("Player"));
        for (int i = 0; i < colliders.Length; i++)
        {
            Interactable interactable = colliders[i].GetComponent<Interactable>();
            if (interactable != null && interactable.HasInteract)
            {
                SetFocus(interactable);
                break;
            }
        }
    }

    protected override void Revive()
    {
        base.Revive();

        transform.position = _startPosition;
        if (isServer)
        {
            Motor.MoveToPoint(_startPosition);
        }
    }

    protected override void Die()
    {
        base.Die();
        if (isServer)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].player.progress.AddExp(_rewardExp / _enemies.Count);
            }
            _enemies.Clear();
        }
    }

    private void Wandering(float deltaTime)
    {
        _changePosTime -= deltaTime;
        if (_changePosTime <= 0)
        {
            RandomMove();
            _changePosTime = Random.Range(_minMoveDelay, _maxMoveDelay);
        }
    }

    private void RandomMove()
    {
        _currentDistanation = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up)
            * new Vector3(_moveRadius, 0.0f, 0.0f) + _startPosition;
        Motor.MoveToPoint(_currentDistanation);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _viewDistance);
    }

    protected override void DamageWithCombat(GameObject user)
    {
        base.DamageWithCombat(user);
        Unit enemy = user.GetComponent<Unit>();
        if (enemy != null)
        {
            SetFocus(enemy.GetComponent<Interactable>());
            Character character = enemy as Character;
            if (character != null && !_enemies.Contains(character))
            {
                _enemies.Add(character);
            }
        }
    }
}