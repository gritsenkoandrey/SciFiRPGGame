using UnityEngine;
using UnityEngine.UI;


public class SkillTreeItem : MonoBehaviour
{
    [SerializeField] private Image _icon = null;
    [SerializeField] private Text _levelText = null;
    [SerializeField] private GameObject _holder = null;

    public void SetSkill(UpgradeableSkill skill)
    {
        if (skill != null)
        {
            _icon.sprite = skill.icon;
            skill.OnSetLevel += ChangeLevel;
            ChangeLevel(skill, skill.Level);
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetUpgradable(bool active)
    {
        _holder.SetActive(active);
    }

    private void ChangeLevel(UpgradeableSkill skill, int newLevel)
    {
        _levelText.text = newLevel.ToString();
    }
}