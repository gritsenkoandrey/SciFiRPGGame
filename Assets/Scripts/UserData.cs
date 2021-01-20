using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class UserData
{
    public Vector3 posCharacter;
    public List<int> inventory = new List<int>();
    public List<int> equipment = new List<int>();

    public int level;
    public int statPoints;
    public float exp;
    public float nextLevelExp;
    public int curHealth;
    public int statDamage;
    public int statArmor;
    public int statMoveSpeed;
}