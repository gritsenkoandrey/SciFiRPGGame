public class PlayerStats : UnitStats
{
    private StatsManager _manager;

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

    public override void OnStartServer()
    {
        base.OnStartServer();
        damage.onStatChanged += DamageChanged;
        armor.onStatChanged += ArmorChanged;
        moveSpeed.onStatChanged += MoveSpeedChanged;
    }

    private void DamageChanged(int value)
    {
        if (_manager != null) _manager.damage = value;
    }

    private void ArmorChanged(int value)
    {
        if (_manager != null) _manager.armor = value;
    }

    private void MoveSpeedChanged(int value)
    {
        if (_manager != null) _manager.moveSpeed = value;
    }
}