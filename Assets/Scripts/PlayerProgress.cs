using UnityEngine;


public class PlayerProgress : MonoBehaviour
{
    private int _level = 1;
    private int _statPoints;
    private int _skillPoints;

    private float _exp;
    private float _nextLevelExp = 100.0f;

    private StatsManager _manager;
    private UserData _data;

    public StatsManager manager
    {
        set
        {
            _manager = value;
            _manager.exp = _exp;
            _manager.nextLevelExp = _nextLevelExp;
            _manager.level = _level;
            _manager.statPoints = _statPoints;
            _manager.skillPoints = _skillPoints;
        }
    }

    public void Load(UserData data)
    {
        _data = data;

        if (data.level > 0)
        {
            _level = data.level;
        }
        _statPoints = data.statPoints;
        _skillPoints = data.skillPoints;
        _exp = data.exp;

        if (data.nextLevelExp > 0)
        {
            _nextLevelExp = data.nextLevelExp;
        }
    }

    public bool RemoveStatPoint()
    {
        if (_statPoints > 0)
        {
            _data.statPoints = --_statPoints;
            if (_manager != null)
            {
                _manager.statPoints = _statPoints;
            }
            return true;
        }
        return false;
    }

    public void AddExp(float addExp)
    {
        _data.exp = _exp += addExp;
        while (_exp >= _nextLevelExp)
        {
            _data.exp = _exp -= _nextLevelExp;
            Debug.Log("LEVEL UP");
            LevelUp();
        }

        if (_manager != null)
        {
            _manager.exp = _exp;
            _manager.level = _level;
            _manager.nextLevelExp = _nextLevelExp;
            _manager.statPoints = _statPoints;
            _manager.skillPoints = _skillPoints;
        }
    }

    public bool RemoveSkillPoint()
    {
        if (_skillPoints > 0)
        {
            _data.skillPoints = --_skillPoints;
            if (_manager != null) _manager.skillPoints = _skillPoints;
            return true;
        }
        return false;
    }

    private void LevelUp()
    {
        _data.level = ++ _level;
        _data.nextLevelExp = _nextLevelExp += 100f;
        _data.statPoints = _statPoints += 3;
        _data.skillPoints = _skillPoints += 1;
    }
}