using System.Collections.Generic;
using UnityEngine.Networking;


public static class AccountManager
{
    private static List<UserAccount> _accounts = new List<UserAccount>();

    public static bool AddAccount(UserAccount account)
    {
        if (_accounts.Find(a => a.login == account.login) == null)
        {
            _accounts.Add(account);
            return true;
        }

        return false;
    }

    public static void RemoveAccount(UserAccount account)
    {
        _accounts.Remove(account);
    }

    public static UserAccount GetAccount(NetworkConnection connection)
    {
        return _accounts.Find(a => a.connection == connection);
    }
}