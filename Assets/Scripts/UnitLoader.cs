using UnityEngine;
using UnityEngine.Networking;


[System.Obsolete]
public class UnitLoader : NetworkBehaviour
{
    [SerializeField] private GameObject _unitPrefab = null;

    public override void OnStartServer()
    {
        base.OnStartServer();
        GameObject unit = Instantiate(_unitPrefab, transform.position, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(unit, gameObject);
    }
}