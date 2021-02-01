using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[Serializable]
public class UserData
{
    public NetworkHash128 characterHash = new NetworkHash128();
    public Vector3 posCharacter;
    public List<int> inventory = new List<int>();
    public List<int> equipment = new List<int>();
    public List<int> skills = new List<int>();

    public int level;
    public int statPoints;
    public int skillPoints;
    public float exp;
    public float nextLevelExp;
    public int curHealth;
    public int statDamage;
    public int statArmor;
    public int statMoveSpeed;
}