using UnityEngine.Networking;


public class StatsManager : NetworkBehaviour
{
    [SyncVar] public int damage;
    [SyncVar] public int armor;
    [SyncVar] public int moveSpeed;

    [SyncVar] public int level;
    [SyncVar] public int statPoints;

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
                case (int)StatType.Damage: player.character.stats.damage.baseValue++;
                    break;
                case (int)StatType.Armor: player.character.stats.armor.baseValue++;
                    break;
                case (int)StatType.MoveSpeed: player.character.stats.moveSpeed.baseValue++;
                    break;
            }
        }
    }
}