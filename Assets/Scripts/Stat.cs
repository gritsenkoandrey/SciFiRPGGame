using UnityEngine;


[System.Serializable]
public class Stat
{
    [SerializeField] private int _baseValue = 0;

    public int GetValue()
    {
        return _baseValue;
    }
}