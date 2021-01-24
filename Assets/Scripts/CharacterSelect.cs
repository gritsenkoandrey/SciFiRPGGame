using System;
using UnityEngine;
using UnityEngine.Networking;


public class CharacterSelect : MonoBehaviour
{
    public static CharacterSelect Instance;
    [SerializeField] private MyNetworkManager _manager = null;

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        _manager.serverRegisterHandler += RegisterServerHandler;
        _manager.clientRegisterHandler += RegisterClientHandler;
    }

    private void RegisterServerHandler()
    {
        NetworkServer.RegisterHandler(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, OnSelectHaracter);
    }

    private void RegisterClientHandler(NetworkClient client)
    {
        client.RegisterHandler(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, OnOpenSelectUI);
    }

    private void OnSelectHaracter(NetworkMessage netMsg)
    {
        NetworkHash128 hash = netMsg.reader.ReadNetworkHash128();
        if (hash.IsValid())
        {
            UserAccount account = AccountManager.GetAccount(netMsg.conn);
            account.data.characterHash = hash;
            _manager.AccountEnter(account);
        }
    }

    private void OnOpenSelectUI(NetworkMessage netMsg)
    {
        CharacterSelectUI.instance.OpenPanel();
    }

    public void SelectCharacter(NetworkHash128 characterHash)
    {
        if (characterHash.IsValid())
        {
            _manager.client.Send(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, new HashMessage(characterHash));
        }
    }
}