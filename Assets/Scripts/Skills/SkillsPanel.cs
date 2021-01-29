using UnityEngine;


public class SkillsPanel : MonoBehaviour
{
    public static SkillsPanel Instance;
    [SerializeField] private SkillPanelItem[] _items = null;
    private UnitSkills _skills;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of SkillsPanel found!");
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (_skills != null)
        {
            bool inCast = _skills.InCast;
            for (int i = 0; i < _skills.Count && i < _items.Length; i++)
            {
                _items[i].SetCastTime(_skills[i].castDelay);
                _items[i].SetHolder(inCast || _skills[i].castDelay > 0 || _skills[i].cooldownDelay > 0);
            }
        }
    }

    public void SetSkills(UnitSkills skills)
    {
        _skills = skills;
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].SetSkill(i < skills.Count ? skills[i] : null);
        }
    }
}