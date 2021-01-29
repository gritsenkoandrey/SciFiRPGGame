using UnityEngine;
using UnityEngine.UI;


public class SkillPanelItem : MonoBehaviour
{
    [SerializeField] private Image _icon = null;
    [SerializeField] private GameObject _holder = null;
    [SerializeField] private Text _timeText = null;

    public void SetSkill(Skill skill)
    {
        if (skill != null)
        {
            _icon.sprite = skill.icon;
            _holder.SetActive(false);
            _timeText.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetHolder(bool active)
    {
        _holder.SetActive(active);
    }

    public void SetCastTime(float time)
    {
        _timeText.text = ((int)time).ToString();
        _timeText.gameObject.SetActive(time > 0);
    }
}