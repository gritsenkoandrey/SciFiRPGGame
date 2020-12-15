using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class Stat
{
    public delegate void StatChanged(int value);
    public event StatChanged onStatChanged;

    [SerializeField] private int _baseValue;

    private List<int> _modifiers = new List<int>();

    public int GetValue()
    {
        int finalValue = _baseValue;
        _modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(int modifier)
    {
        if (modifier != 0)
        {
            _modifiers.Add(modifier);
            if (onStatChanged != null)
            {
                onStatChanged(GetValue());
            }
        }
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
        {
            _modifiers.Remove(modifier);
            if (onStatChanged != null)
            {
                onStatChanged(GetValue());
            }
        }
    }
}