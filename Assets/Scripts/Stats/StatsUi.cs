using UnityEngine;
using UnityEngine.UI;

public class StatsUi : MonoBehaviour
{
    public static StatsUi Instance;

    [SerializeField] private GameObject _statsUI;
    [SerializeField] private StatItem _damageStat;
    [SerializeField] private StatItem _armorStat;
    [SerializeField] private StatItem _moveSpeedStat;

    [SerializeField] private Text _levelText;
    [SerializeField] private Text _statPointsText;

    private StatsManager _manager;

    private int _curDamage;
    private int _curArmor;
    private int _curMoveSpeed;

    private int _curLevel;
    private int _curStatPoints;

    private float _curExp;
    private float _nextLevelExp;

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

        if (_curLevel != _manager.level)
        {
            _curLevel = _manager.level;
            _levelText.text = _curLevel.ToString();
        }

        if (_curExp != _manager.exp)
        {
            _curExp = _manager.exp;
        }

        if (_nextLevelExp != _manager.nextLevelExp)
        {
            _nextLevelExp = _manager.nextLevelExp;
        }

        if (_curStatPoints != _manager.statPoints)
        {
            _curStatPoints = _manager.statPoints;
            _statPointsText.text = _curStatPoints.ToString();

            if (_curStatPoints > 0)
            {
                SetUpgradableStats(true);
            }
            else
            {
                SetUpgradableStats(false);
            }
        }
    }

    private void SetUpgradableStats(bool active)
    {
        _damageStat.SetUpgradable(active);
        _armorStat.SetUpgradable(active);
        _moveSpeedStat.SetUpgradable(active);
    }

    public void UpgradeStat(StatItem stat)
    {
        if (stat == _damageStat)
        {
            _manager.CmdUpgradeStat((int)StatType.Damage);
        }
        else if (stat == _armorStat)
        {
            _manager.CmdUpgradeStat((int)StatType.Armor);
        }
        else if (stat == _moveSpeedStat)
        {
            _manager.CmdUpgradeStat((int)StatType.MoveSpeed);
        }
    }
}