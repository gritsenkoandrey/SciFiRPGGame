using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerVisibility : NetworkBehaviour
{
    [SerializeField] private float _visRange = 10.0f;
    [SerializeField] private float _visUpdateInterval = 1.0f;
    [SerializeField] private LayerMask _visMask;

    private Transform _transform;
    private float _visUpdateTime;

    public override void OnStartServer()
    {
        _transform = transform;
    }

    void Update()
    {
        if (isServer)
        {
            if (Time.time - _visUpdateTime > _visUpdateInterval)
            {
                GetComponent<NetworkIdentity>().RebuildObservers(false);
                _visUpdateTime = Time.time;
            }
        }

    }

    public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
    {
        Collider[] hits = Physics.OverlapSphere(_transform.position, _visRange, _visMask);
        foreach (Collider hit in hits)
        {
            Character character = hit.GetComponent<Character>();
            if (character != null && character.player != null)
            {
                NetworkIdentity identity = character.player.GetComponent<NetworkIdentity>();
                if (identity != null && identity.connectionToClient != null)
                {
                    observers.Add(identity.connectionToClient);
                }
            }
        }
        // если это персонаж то он всегда видим для своего игрока
        Character m_character = GetComponent<Character>();
        if (m_character != null && !observers.Contains(m_character.player.connection))
        {
            observers.Add(m_character.player.connection);
        }
        return true;
    }

    public override bool OnCheckObserver(NetworkConnection connection)
    {
        // если это персонаж то он всегда видим для своего игрока
        Character character = GetComponent<Character>();
        if (character != null && connection == character.player.connection)
        {
            return true;
        }
        // находим объект игрока по коннекту
        Player player = null;
        foreach (UnityEngine.Networking.PlayerController controller in connection.playerControllers)
        {
            if (controller != null)
            {
                player = controller.gameObject.GetComponent<Player>();
                if (player != null) break;
            }
        }
        // если игрок в зоне видимости то объект видим для него
        if (player != null && player.character != null)
        {
            return (player.character.transform.position - _transform.position).magnitude < _visRange;
        }
        else return false;
    }
}