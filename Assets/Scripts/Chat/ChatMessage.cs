using System;
using UnityEngine.Networking;


[Serializable]
public struct ChatMessage
{
    public NetworkInstanceId senderId;
    public NetworkInstanceId reciverId;
    public string author;
    public string message;

    public ChatMessage(NetworkInstanceId sender, NetworkInstanceId reciver, string author, string message)
    {
        senderId = sender;
        reciverId = reciver;
        this.author = author;
        this.message = message;
    }
}
