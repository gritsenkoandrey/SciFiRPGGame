public class PlayerStats : UnitStats
{
    private StatsManager _manager;
    private UserData _data;

    public StatsManager manager
    {
        set
        {
            _manager = value;
            _manager.damage = damage.GetValue();
            _manager.armor = armor.GetValue();
            _manager.moveSpeed = moveSpeed.GetValue();
        }
    }

    public override int CurrentHealth
    {
        get
        {
            return base.CurrentHealth;
        }
        protected set
        {
            base.CurrentHealth = value;
            _data.curHealth = CurrentHealth;
        }
    }

    public void Load(UserData data)
    {
        _data = data;
        CurrentHealth = data.curHealth;
        if (data.statDamage > 0) damage.baseValue = data.statDamage;
        if (data.statArmor > 0) armor.baseValue = data.statArmor;
        if (data.statMoveSpeed > 0) moveSpeed.baseValue = data.statMoveSpeed;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        damage.onStatChanged += DamageChanged;
        armor.onStatChanged += ArmorChanged;
        moveSpeed.onStatChanged += MoveSpeedChanged;
    }

    private void DamageChanged(int value)
    {
        if (damage.baseValue != _data.statDamage)
        {
            _data.statDamage = damage.baseValue;
        }

        if (_manager != null)
        {
            _manager.damage = value;
        }
    }

    private void ArmorChanged(int value)
    {
        if (armor.baseValue != _data.statArmor)
        {
            _data.statArmor = armor.baseValue;
        }

        if (_manager != null)
        {
            _manager.armor = value;
        }
    }

    private void MoveSpeedChanged(int value)
    {
        if (moveSpeed.baseValue != _data.statMoveSpeed)
        {
            _data.statMoveSpeed = moveSpeed.baseValue;
        }

        if (_manager != null)
        {
            _manager.moveSpeed = value;
        }
    }
}