using UnityEngine;
using UnityEngine.Networking;


public class LoginUI : MonoBehaviour
{
    [SerializeField] private GameObject _loginPanel = null;

    private void Start()
    {
        if ((NetworkManager.singleton as MyNetworkManager).serverMode)
        {
            _loginPanel.SetActive(false);
        }
    }

    public void Login()
    {
        NetworkManager.singleton.StartClient();
    }
}