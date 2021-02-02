using UnityEngine;


public class GlobalChatChannel : ChatChannel
{
    public static ChatChannel instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of GlobalChat found!");
            return;
        }
        instance = this;
    }
}
