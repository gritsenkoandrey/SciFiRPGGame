using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class ChatUI : MonoBehaviour
{
    public static ChatUI Instance;

    [SerializeField] private Dropdown _channelsDropdown= null;
    [SerializeField] private InputField _messageInput = null;
    [SerializeField] private PrivateChatChannel _privateChannel = null;
    [SerializeField] private Transform _massageContaner = null;
    [SerializeField] private ChatMessageUI _massagePrefab = null;
    private PlayerChat _playerChat;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of ChatUI found!");
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (_messageInput.isFocused && _messageInput.text != "" && Input.GetKey(KeyCode.Return))
        {
            SendChatMessage(_messageInput.text);
            _messageInput.text = "";
        }
    }

    public void SetPlayerChat(PlayerChat chat)
    {
        _playerChat = chat;
        RefreshChanels();
        _playerChat.OnChangeChannels += RefreshChanels;
        _playerChat.OnReciveMessage += ReciveChatMessage;
    }


    private void RefreshChanels()
    {
        _channelsDropdown.ClearOptions();
        _channelsDropdown.AddOptions(_playerChat.channels.ConvertAll(x => x.name));
    }

    public void SendChatMessage(string text)
    {
        if (_playerChat != null) _playerChat.channels[_channelsDropdown.value].SendFromPlayerChat(text);
    }

    public void ReciveChatMessage(ChatMessage message)
    {
        ChatMessageUI newMessage = Instantiate(_massagePrefab, _massageContaner);
        newMessage.SetChatMessage(message);
    }

    public void SetPrivateMessage(ChatMessage message)
    {
        _privateChannel.SetReciverMessage(message);
        if (_playerChat.channels.Contains(_privateChannel))
        {
            RefreshChanels();
        }
        else
        {
            _playerChat.RegisterChannel(_privateChannel);
        }
        _channelsDropdown.value = _channelsDropdown.options.Count - 1;
    }
}
