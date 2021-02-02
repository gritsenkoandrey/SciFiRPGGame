using UnityEngine.Networking;


public class PrivateChatChannel : ChatChannel
{
    private NetworkInstanceId _reciver;

    public void SetReciverMessage(ChatMessage message)
    {
        _reciver = message.senderId;
        name = message.author;
    }

    public override void SendFromPlayerChat(string text)
    {
        ChatMessage msg = new ChatMessage(PlayerChat.Instance.netId, _reciver, PlayerChat.Instance.name, text);
        PlayerChat.Instance.CmdSendFromChannel(gameObject, msg);
    }

    public override void SendFromChanel(ChatMessage message)
    {
        TargetSendFromChanel(NetworkServer.objects[message.reciverId].connectionToClient, message);
    }

    [TargetRpc]
    protected void TargetSendFromChanel(NetworkConnection target, ChatMessage message)
    {
        PlayerChat.Instance.ReciveChatMessage(message);
    }
}
