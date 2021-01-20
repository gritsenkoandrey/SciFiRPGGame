using DatabaseControl;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml.Serialization;
using System.IO;


public class UserAccount
{
    public string login;
    public string pass;
    public UserData data;
    public NetworkConnection connection;

    //private static XmlSerializer _xmlSerializer;

    public UserAccount (NetworkConnection connection)
    {
        this.connection = connection;
        //_xmlSerializer = new XmlSerializer(typeof(UserData));
    }

    public IEnumerator Login(string login, string pass)
    {
        IEnumerator eLogin = DCF.Login(login, pass);
        while (eLogin.MoveNext())
        {
            yield return eLogin.Current;
        }
        string response = eLogin.Current as string;

        if (response == "Success")
        {
            Debug.Log("Server login success");
            this.login = login;
            this.pass = pass;

            if (AccountManager.AddAccount(this))
            {
                IEnumerator eLoad = LoadData();
                while (eLoad.MoveNext())
                {
                    yield return eLoad.Current;
                }
                response = eLoad.Current as string;

                if (response == "Error")
                {
                    yield return eLoad.Current;
                }
                else
                {
                    yield return eLogin.Current;
                }
            }
            else
            {
                Debug.Log("Account already use");
                yield return "Already use";
            }
        }
        else
        {
            Debug.Log("Server login fail");
            yield return eLogin.Current;
        }
    }

    public IEnumerator Quit()
    {
        IEnumerator eSave = SaveData();
        while (eSave.MoveNext())
        {
            yield return eSave.Current;
        }
        AccountManager.RemoveAccount(this);
    }

    private IEnumerator LoadData()
    {
        IEnumerator e = DCF.GetUserData(login, pass);
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string;
        
        if (response == "Error")
        {
            Debug.Log("UserData for user " + login + " load error with code: " + response);
        }
        else
        {
            Debug.Log("UserData for user " + login + " completely load.");
            Debug.Log(response);
            if (response != "")
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserData));
                data = (UserData)xmlSerializer.Deserialize(new StringReader(response));
            }
            else
            {
                //data = new UserData();
                data = new UserData
                {
                    posCharacter = new Vector3(250.0f, 0.0f, 250.0f)
                };
                Debug.Log("Player spawn" + data.posCharacter);
            }
        }
    }

    private IEnumerator SaveData()
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserData));
        StringWriter writer = new StringWriter();
        xmlSerializer.Serialize(writer, data);
        IEnumerator e = DCF.SetUserData(login, pass, writer.ToString());
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string;

        if (response == "Success")
        {
            Debug.Log("UserData for user " + login + " completely save.");
        }
        else
        {
            Debug.LogError("UserData for user " + login + " save error with code: " + response);
        }
    }
}