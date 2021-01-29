using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class Unit : Interactable
{
    [SerializeField] private UnitMotor _unitMotor;
    [SerializeField] private UnitStats _unitStats;
    public UnitStats Stats { get { return _unitStats; } }
    public UnitMotor Motor { get { return _unitMotor; } }
    public UnitSkills unitSkills;

    protected Interactable focus;
    public Interactable Focus { get { return focus; } }
    protected float interactDistance;
    protected bool isDead;

    public delegate void UnitDelegate();
    public event UnitDelegate EventOnDamage;
    public event UnitDelegate EventOnDie;
    public event UnitDelegate EventOnRevive;

    public override void OnStartServer()
    {
        _unitMotor.SetMoveSpeed(_unitStats.moveSpeed.GetValue());
        _unitStats.moveSpeed.onStatChanged += _unitMotor.SetMoveSpeed;
    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnAliveUpdate() { }
    protected virtual void OnDeadUpdate() { }

    protected void OnUpdate()
    {
        if (isServer)
        {
            if (!isDead)
            {
                if (_unitStats.CurrentHealth == 0)
                {
                    Die();
                }
                else
                {
                    OnAliveUpdate();
                }
            }
            else
            {
                OnDeadUpdate();
            }
        }
    }

    public void UseSkill(int skillNum)
    {
        if (!isDead && skillNum < unitSkills.Count)
        {
            unitSkills[skillNum].Use(this);
        }
    }

    public override float GetInteractDistance(GameObject user)
    {
        Combat combat = user.GetComponent<Combat>();
        return base.GetInteractDistance(user) + (combat != null ? combat.attackDistance : 0.0f);
    }

    public override bool Interact(GameObject user)
    {
        Combat combat = user.GetComponent<Combat>();
        if (combat != null)
        {
            if (combat.Attack(_unitStats))
            {
                DamageWithCombat(user);
            }
            return true;
        }
        return base.Interact(user);
    }

    public virtual void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            focus = newFocus;
            interactDistance = focus.GetInteractDistance(gameObject);
            _unitMotor.FollowTarget(newFocus, interactDistance);
        }
    }

    public virtual void RemoveFocus()
    {
        focus = null;
        _unitMotor.StopFollowingTarget();
    }

    protected virtual void DamageWithCombat(GameObject user)
    {
        EventOnDamage();
    }

    public void TakeDamage(GameObject user, int damage)
    {
        _unitStats.TakeDamage(damage);
        DamageWithCombat(user);
    }

    protected virtual void Die()
    {
        isDead = true;
        GetComponent<Collider>().enabled = false;
        EventOnDie();
        if (isServer)
        {
            HasInteract = false;
            RemoveFocus();
            _unitMotor.MoveToPoint(transform.position);
            RpcDie();
        }
    }

    [ClientRpc]
    private void RpcDie()
    {
        if (!isServer)
        {
            Die();
        }
    }

    protected virtual void Revive()
    {
        isDead = false;
        GetComponent<Collider>().enabled = true;
        EventOnRevive();
        if (isServer)
        {
            HasInteract = true;
            _unitStats.SetHealthRate(1.0f);
            RpcReviev();
        }
    }

    [ClientRpc]
    private void RpcReviev()
    {
        if (!isServer)
        {
            Revive();
        }
    }
}