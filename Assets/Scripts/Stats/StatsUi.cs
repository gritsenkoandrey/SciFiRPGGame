using UnityEngine;


public class StatsUi : MonoBehaviour
{
    public static StatsUi Instance;

    [SerializeField] private GameObject _statsUI;
    [SerializeField] private StatItem _damageStat;
    [SerializeField] private StatItem _armorStat;
    [SerializeField] private StatItem _moveSpeedStat;

    private StatsManager _manager;

    private int _curDamage;
    private int _curArmor;
    private int _curMoveSpeed;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of StatsUI found!");
            return;
        }
        Instance = this;
    }

    void Start()
    {
        _statsUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Stats"))
        {
            _statsUI.SetActive(!_statsUI.activeSelf);
        }
        if (_manager != null)
        {
            CheckManagerChanges();
        }
    }

    public void SetManager(StatsManager statsManager)
    {
        _manager = statsManager;
        CheckManagerChanges();
    }

    private void CheckManagerChanges()
    {
        if (_curDamage != _manager.damage)
        {
            _curDamage = _manager.damage;
            _damageStat.ChangeStat(_curDamage);
        }
        if (_curArmor != _manager.armor)
        {
            _curArmor = _manager.armor;
            _armorStat.ChangeStat(_curArmor);
        }
        if (_curMoveSpeed != _manager.moveSpeed)
        {
            _curMoveSpeed = _manager.moveSpeed;
            _moveSpeedStat.ChangeStat(_curMoveSpeed);
        }
    }
}