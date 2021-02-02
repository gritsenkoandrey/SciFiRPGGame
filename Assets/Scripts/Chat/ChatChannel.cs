using UnityEngine;
using UnityEngine.Networking;


public class ChatChannel : NetworkBehaviour
{
    public new string name;

    public virtual void SendFromChanel(ChatMessage message)
    {
        RpcSendFromChanel(message);
    }

    [ClientRpc]
    protected void RpcSendFromChanel(ChatMessage message)
    {
        PlayerChat.Instance.ReciveChatMessage(message);
    }

    public virtual void SendFromPlayerChat(string text)
    {
        var msg = new ChatMessage(PlayerChat.Instance.netId, NetworkInstanceId.Invalid, PlayerChat.Instance.name, text);
        PlayerChat.Instance.CmdSendFromChannel(gameObject, msg);
    }

    //NetworkInstanceId – это структура, которая содержит идентификатор объекта,
    //созданного на стороне сервера.По нему без лишнего труда можно обратиться к сетевому объекту,
    //владеющему данным идентификатором.
}
