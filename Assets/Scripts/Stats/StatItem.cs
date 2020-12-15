using UnityEngine;
using UnityEngine.UI;


public class StatItem : MonoBehaviour
{
    [SerializeField] private Text _value;

    public void ChangeStat(int stat)
    {
        _value.text = stat.ToString();
    }
}