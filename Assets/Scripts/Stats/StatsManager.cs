using UnityEngine.Networking;


public class StatsManager : NetworkBehaviour
{
    [SyncVar] public int damage;
    [SyncVar] public int armor;
    [SyncVar] public int moveSpeed;

    [SyncVar] public int level;
    [SyncVar] public int statPoints;
    [SyncVar] public int skillPoints;

    [SyncVar] public float exp;
    [SyncVar] public float nextLevelExp;

    public Player player;

    [Command]
    public void CmdUpgradeStat(int stat)
    {
        if (player.progress.RemoveStatPoint())
        {
            switch(stat)
            {
                case (int)StatType.Damage: player.character.Stats.damage.baseValue++;
                    break;
                case (int)StatType.Armor: player.character.Stats.armor.baseValue++;
                    break;
                case (int)StatType.MoveSpeed: player.character.Stats.moveSpeed.baseValue++;
                    break;
            }
        }
    }

    [Command]
    public void CmdUpgradeSkill(int index)
    {
        if (player.progress.RemoveSkillPoint())
        {
            UpgradeableSkill skill = player.character.unitSkills[index] as UpgradeableSkill;
            if (skill != null)
            {
                skill.Level++;
            }
        }
    }
}