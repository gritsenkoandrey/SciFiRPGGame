using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private GameObject _currentPanel = null;
    [SerializeField] private GameObject _loginPanel = null;
    [SerializeField] private GameObject _registerPanel = null;
    [SerializeField] private GameObject _loadingPanel = null;
    [SerializeField] private InputField _loginLogin = null;
    [SerializeField] private InputField _loginPass = null;
    [SerializeField] private InputField _registerLogin = null;
    [SerializeField] private InputField _registerPass = null;
    [SerializeField] private InputField _registerConfirm = null;

    private MyNetworkManager _myNetworkManager;

    private void Start()
    {
        _myNetworkManager = NetworkManager.singleton as MyNetworkManager;

        if (_myNetworkManager.serverMode)
        {
            _loadingPanel.SetActive(false);
        }
        else
        {
            _myNetworkManager.loginResponseDelegate = LoginResponse;
            _myNetworkManager.registerResponseDelegate = RegisterResponse;
        }
    }

    private void ClearInputs()
    {
        _loginLogin.text = "";
        _loginPass.text = "";
        _registerLogin.text = "";
        _registerPass.text = "";
        _registerConfirm.text = "";
    }

    public void Login()
    {
        _myNetworkManager.Login(_loginLogin.text, _loginPass.text);
        _currentPanel.SetActive(false);
        _loadingPanel.SetActive(true);
    }

    public void Register()
    {
        if (_registerPass.text != "" && _registerPass.text == _registerConfirm.text)
        {
            _myNetworkManager.Register(_registerLogin.text, _registerPass.text);
            _currentPanel.SetActive(false);
            _loadingPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Error: Password Incorrect");
            ClearInputs();
        }
    }

    public void LoginResponse(string response)
    {
        switch (response)
        {
            case "UserError":
                Debug.Log("Error: Username not Found");
                break;
            case "PassError":
                Debug.Log("Error: Password Incorrect");
                break;
            default:
                Debug.Log("Error: Unknown Error. Please try again later.");
                break;
        }

        _loadingPanel.SetActive(false);
        _currentPanel.SetActive(true);
        ClearInputs();
    }

    public void RegisterResponse(string response)
    {
        switch (response)
        {
            case "Success":
                Debug.Log("User registered");
                break;
            case "UserError":
                Debug.Log("Error: Username Already Taken");
                break;
            default:
                Debug.Log("Error: Unknown Error. Please try again later.");
                break;
        }

        _loadingPanel.SetActive(false);
        _currentPanel.SetActive(true);
        ClearInputs();
    }

    public void SetPanel(GameObject panel)
    {
        _currentPanel.SetActive(false);
        _currentPanel = panel;
        _currentPanel.SetActive(true);
        ClearInputs();
    }
}