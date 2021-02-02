using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerChat : NetworkBehaviour
{
    public static PlayerChat Instance;

    public override void OnStartClient()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of PlayerChat found!");
            return;
        }
        Instance = this;
    }

    public List<ChatChannel> channels = new List<ChatChannel>();

    public delegate void ChangeChannelsDelegate();
    public event ChangeChannelsDelegate OnChangeChannels;

    public delegate void ReciveMessageDelegate(ChatMessage message);
    public event ReciveMessageDelegate OnReciveMessage;

    public void RegisterChannel(ChatChannel channel)
    {
        channels.Add(channel);
        OnChangeChannels?.Invoke();
    }

    [Command]
    public void CmdSendFromChannel(GameObject channelGO, ChatMessage message)
    {
        message.author = AccountManager.GetAccount(connectionToClient).login;
        channelGO.GetComponent<ChatChannel>().SendFromChanel(message);
    }

    public void ReciveChatMessage(ChatMessage message)
    {
        OnReciveMessage?.Invoke(message);
    }
}
