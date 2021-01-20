using UnityEngine;


[RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
public class Character : Unit
{
    [SerializeField] private float _reviveDelay = 5f;
    [SerializeField] private GameObject _gfx;

    private Vector3 _startPosition;
    private float _reviveTime;
    public Player player;

    new public PlayerStats stats { get { return _unitStats as PlayerStats; } }

    void Start()
    {
        _startPosition = new Vector3(250.0f, 0.0f, 250.0f);
        _reviveTime = _reviveDelay;

        if (stats.CurrentHealth == 0)
        {
            transform.position = _startPosition;
            if (isServer)
            {
                stats.SetHealthRate(1);
                _unitMotor.MoveToPoint(_startPosition);
            }
        }
    }

    void Update()
    {
        OnUpdate();
    }

    protected override void OnDeadUpdate()
    {
        base.OnDeadUpdate();

        if (_reviveTime > 0)
        {
            _reviveTime -= Time.deltaTime;
        }
        else
        {
            _reviveTime = _reviveDelay;
            Revive();
        }
    }

    protected override void OnAliveUpdate()
    {
        base.OnAliveUpdate();

        if (focus != null)
        {
            if (!focus.HasInteract)
            {
                RemoveFocus();
            }
            else
            {
                float distance = Vector3.Distance(focus.interactionTransform.position, transform.position);
                if (distance <= focus.radius)
                {
                    if (!focus.Interact(gameObject))
                    {
                        RemoveFocus();
                    }
                }
            }
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
            RemoveFocus();
            _unitMotor.MoveToPoint(point);
        }
    }

    public void SetNewFocus(Interactable newFocus)
    {
        if (!isDead)
        {
            if (newFocus.HasInteract)
            {
                SetFocus(newFocus);
            }
        }
    }
}