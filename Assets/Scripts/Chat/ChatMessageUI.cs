using UnityEngine;
using UnityEngine.UI;


public class ChatMessageUI : MonoBehaviour
{
    [SerializeField] private Text _authorText = null;
    [SerializeField] private Text _messageText = null;
    [SerializeField] private Button _privateButton = null;

    private ChatMessage _msg;

    public void SetChatMessage(ChatMessage message)
    {
        _msg = message;
        _authorText.text = message.author;
        _messageText.text = message.message;
        _privateButton.onClick.AddListener(SetPrivateMessage);
    }

    public void SetPrivateMessage()
    {
        ChatUI.Instance.SetPrivateMessage(_msg);
    }
}
