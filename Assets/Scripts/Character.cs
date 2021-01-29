using UnityEngine;


[RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
public class Character : Unit
{
    [SerializeField] private float _reviveDelay = 5.0f;
    [SerializeField] private GameObject _gfx;

    private Vector3 _startPosition;
    private float _reviveTime;
    public Player player;

    new public PlayerStats Stats { get { return base.Stats as PlayerStats; } }

    void Start()
    {
        _startPosition = new Vector3(250.0f, 0.0f, 250.0f);
        _reviveTime = _reviveDelay;

        if (Stats.CurrentHealth == 0)
        {
            transform.position = _startPosition;
            if (isServer)
            {
                Stats.SetHealthRate(1);
                Motor.MoveToPoint(_startPosition);
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
                if (distance <= interactDistance)
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
            Motor.MoveToPoint(_startPosition);
        }
    }

    public void SetMovePoint(Vector3 point)
    {
        if (!isDead)
        {
            RemoveFocus();
            Motor.MoveToPoint(point);
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