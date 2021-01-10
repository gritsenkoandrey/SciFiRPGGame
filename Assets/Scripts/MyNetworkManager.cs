using DatabaseControl;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;


public class MyNetworkManager : NetworkManager
{
    public delegate void ResponseDelegate(string response);
    public ResponseDelegate loginResponseDelegate;
    public ResponseDelegate registerResponseDelegate;

    public bool serverMode;

    private void Start()
    {
        if (serverMode)
        {
            StartServer();
            NetworkServer.UnregisterHandler(MsgType.Connect);
            NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnectCustom);
            NetworkServer.RegisterHandler(MsgType.Highest + (short)NetMsgType.Login, OnServerLogin);
            NetworkServer.RegisterHandler(MsgType.Highest + (short)NetMsgType.Register, OnServerRegister);
        }
    }

    private void OnServerConnectCustom(NetworkMessage netMsg)
    {
        if (LogFilter.logDebug)
        {
            Debug.Log("NetworkManager:OnServerConnectCustom");
        }

        netMsg.conn.SetMaxDelay(maxDelay);
        OnServerConnect(netMsg.conn);
    }

    // получение сообщений логина на сервере
    private void OnServerLogin(NetworkMessage netMsg)
    {
        StartCoroutine(LoginUser(netMsg));
    }

    private void OnServerRegister(NetworkMessage netMsg)
    {
        StartCoroutine(RegisterUser(netMsg));
    }

    // получение сообщений логина на клиенте
    private void OnClientLogin(NetworkMessage netMsg)
    {
        loginResponseDelegate.Invoke(netMsg.reader.ReadString());
    }

    private void OnClientRegister(NetworkMessage netMsg)
    {
        registerResponseDelegate.Invoke(netMsg.reader.ReadString());
    }

    // логин игрока на сервере
    private IEnumerator LoginUser(NetworkMessage netMsg)
    {
        UserMessage msg = netMsg.ReadMessage<UserMessage>();
        IEnumerator e = DCF.Login(msg.login, msg.password);

        while (e.MoveNext())
        {
            yield return e.Current;
        }

        string response = e.Current as string;

        if (response == "Success")
        {
            Debug.Log("server login success");
            netMsg.conn.Send(MsgType.Scene, new StringMessage(SceneManager.GetActiveScene().name));
        }
        else
        {
            Debug.Log("server login fail");
            netMsg.conn.Send(MsgType.Highest + (short)NetMsgType.Login, new StringMessage(response));
        }
    }

    // регистрация игрока на сервере
    private IEnumerator RegisterUser(NetworkMessage netMsg)
    {
        UserMessage msg = netMsg.ReadMessage<UserMessage>();
        IEnumerator e = DCF.RegisterUser(msg.login, msg.password, "");

        while (e.MoveNext())
        {
            yield return e.Current;
        }

        string response = e.Current as string;

        Debug.Log("server register done");
        netMsg.conn.Send(MsgType.Highest + (short)NetMsgType.Register, new StringMessage(response));
    }

    // методы, вызываемые UI для отправки запроса
    public void Login(string login, string password)
    {
        ClientConnect();
        StartCoroutine(SendLogin(login, password));
    }

    public void Register(string login, string password)
    {
        ClientConnect();
        StartCoroutine(SendRegister(login, password));
    }

    // корутины с ожиданием подключения клиента
    private IEnumerator SendLogin(string login, string password)
    {
        while (!client.isConnected)
        {
            yield return null;
        }

        Debug.Log("client login");
        client.connection.Send(MsgType.Highest + (short)NetMsgType.Login, new UserMessage(login, password));
    }

    private IEnumerator SendRegister(string login, string password)
    {
        while (!client.isConnected)
        {
            yield return null;
        }

        Debug.Log("client register");
        client.connection.Send(MsgType.Highest + (short)NetMsgType.Register, new UserMessage(login, password));
    }

    private void ClientConnect()
    {
        NetworkClient client = this.client;
        if (client == null)
        {
            client = StartClient();
            client.RegisterHandler(MsgType.Highest + (short)NetMsgType.Login, OnClientLogin);
            client.RegisterHandler(MsgType.Highest + (short)NetMsgType.Register, OnClientRegister);
        }
    }

    #region Command

    //public override void OnServerConnect(NetworkConnection conn)
    //{
    //    Debug.Log("A client connected to the server: " + conn);
    //}

    //public override void OnServerDisconnect(NetworkConnection conn)
    //{
    //    NetworkServer.DestroyPlayersForConnection(conn);

    //    if (conn.lastError != NetworkError.Ok)
    //    {
    //        if (LogFilter.logError)
    //        {
    //            Debug.LogError("ServerDisconnected due to error: " + conn.lastError);
    //        }
    //    }
    //    Debug.Log("A client disconnected from the server: " + conn);
    //}

    //public override void OnServerReady(NetworkConnection conn)
    //{
    //    NetworkServer.SetClientReady(conn);
    //    Debug.Log("Client is set to the ready state (ready to receive state updates): " + conn);
    //}

    //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    //    NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    //    Debug.Log("Client has requested to get his player added to the game");
    //}
    //public override void OnServerRemovePlayer(NetworkConnection conn, UnityEngine.Networking.PlayerController player)
    //{
    //    if (player.gameObject != null)
    //    {
    //        NetworkServer.Destroy(player.gameObject);
    //    }
    //}

    //public override void OnServerError(NetworkConnection conn, int errorCode)
    //{
    //    Debug.Log("Server network error occurred: " + (NetworkError)errorCode);
    //}

    //public override void OnStartHost()
    //{
    //    Debug.Log("Host has started");
    //}

    //public override void OnStartServer()
    //{
    //    Debug.Log("Server has started");
    //}

    //public override void OnStopServer()
    //{
    //    Debug.Log("Server has stopped");
    //}

    //public override void OnStopHost()
    //{
    //    Debug.Log("Host has stopped");
    //}

    //// Client callbacks

    //public override void OnClientConnect(NetworkConnection conn)
    //{
    //    base.OnClientConnect(conn);
    //    Debug.Log("Connected successfully to server, now to set up other stuff for the client...");
    //}

    //public override void OnClientDisconnect(NetworkConnection conn)
    //{
    //    StopClient();
    //    if (conn.lastError != NetworkError.Ok)
    //    {
    //        if (LogFilter.logError)
    //        {
    //            Debug.LogError("ClientDisconnected due to error: " + conn.lastError);
    //        }
    //    }
    //    Debug.Log("Client disconnected from server: " + conn);
    //}

    //public override void OnClientError(NetworkConnection conn, int errorCode)
    //{
    //    Debug.Log("Client network error occurred: " + (NetworkError)errorCode);
    //}

    //public override void OnClientNotReady(NetworkConnection conn)
    //{
    //    Debug.Log("Server has set client to be not-ready (stop getting state updates)");
    //}

    //public override void OnStartClient(NetworkClient client)
    //{
    //    Debug.Log("Client has started");
    //}

    //public override void OnStopClient()
    //{
    //    Debug.Log("Client has stopped");
    //}

    //public override void OnClientSceneChanged(NetworkConnection conn)
    //{
    //    base.OnClientSceneChanged(conn);
    //    Debug.Log("Server triggered scene change and we've done the same, do any extra work here for the client...");
    //}

    #endregion
}