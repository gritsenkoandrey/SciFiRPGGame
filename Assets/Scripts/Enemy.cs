using UnityEngine;


[System.Obsolete, RequireComponent(typeof(UnitMotor), typeof(EnemyStats))]
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
    [SerializeField] private float _viewDistance = 5.0f;
    [SerializeField] private float _revievDelay = 5.0f;
    private float _revievTime;

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

        Wandering(Time.deltaTime);
    }

    protected override void Revive()
    {
        base.Revive();

        transform.position = _startPosition;
        if (isServer)
        {
            _unitMotor.MoveToPoint(_startPosition);
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
        _currentDistanation = Quaternion.AngleAxis(Random.Range(0.0f, 45.0f), Vector3.up)
            * new Vector3(_moveRadius, 0.0f, 0.0f) + _startPosition;
        _unitMotor.MoveToPoint(_currentDistanation);
    }
}