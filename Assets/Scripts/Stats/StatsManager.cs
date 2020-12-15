using UnityEngine.Networking;


public class StatsManager : NetworkBehaviour
{
    [SyncVar] public int damage;
    [SyncVar] public int armor;
    [SyncVar] public int moveSpeed;
}