using System;
using UnityEngine;


[Serializable]
public class UnitSkills
{
    [SerializeField] private Skill[] _skills = null;

    public Skill this [int index]
    {
        get { return _skills[index]; }
        set { _skills[index] = value; }
    }

    public int Count { get { return _skills.Length; } }

    public bool InCast
    {
        get
        {
            foreach (var skill in _skills)
            {
                if (skill.castDelay > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}