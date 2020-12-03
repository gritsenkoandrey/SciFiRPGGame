using UnityEngine;


[System.Obsolete, RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
public class Character : Unit
{
    [SerializeField] private float _revievDelay = 5.0f;
    [SerializeField] private GameObject _gfx;

    private Vector3 _startPosition;
    private float _revievTime;

    private void Start()
    {
        _startPosition = transform.position;
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

    protected override void Die()
    {
        base.Die();

        _gfx.SetActive(false);
    }

    protected override void Revive()
    {
        base.Revive();

        transform.position = _startPosition;
        _gfx.SetActive(true);
        if (isServer)
        {
            _unitMotor.MoveToPoint(_startPosition);
        }
    }

    public void SetMovePoint(Vector3 point)
    {
        if (!isDead)
        {
            _unitMotor.MoveToPoint(point);
        }
    }
}