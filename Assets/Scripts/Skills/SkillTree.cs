using UnityEngine;
using UnityEngine.UI;


public class SkillTree : MonoBehaviour
{
    public static SkillTree Instance;

    [SerializeField] private SkillTreeItem[] _items = null;
    [SerializeField] private Text _skillPointsText = null;

    private StatsManager _manager;
    private int _curSkillPoints;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of SkillTree found!");
            return;
        }
        Instance = this;
    }

    void Update()
    {
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

    public void SetCharacter(Character character)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].SetSkill(i < character.unitSkills.Count ? character.unitSkills[i] as UpgradeableSkill : null);
        }
        if (_manager != null) CheckManagerChanges();
    }

    private void CheckManagerChanges()
    {
        if (_curSkillPoints != _manager.skillPoints)
        {
            _curSkillPoints = _manager.skillPoints;
            _skillPointsText.text = _curSkillPoints.ToString();
            if (_curSkillPoints > 0) SetUpgradableSkills(true);
            else SetUpgradableSkills(false);
        }
    }

    private void SetUpgradableSkills(bool active)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].SetUpgradable(active);
        }
    }

    public void UpgradeSkill(SkillTreeItem skillItem)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] == skillItem)
            {
                _manager.CmdUpgradeSkill(i);
                break;
            }
        }
    }
}