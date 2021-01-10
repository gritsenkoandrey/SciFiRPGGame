using UnityEngine.Networking;


public class UserMessage : MessageBase
{
    // передаваемые поля
    public string login;
    public string password;

    // конструктор, обязательный для работы Unet с наследником
    public UserMessage()
    {

    }

    // конструктор для удобной отправки данных
    public UserMessage(string login, string password)
    {
        this.login = login;
        this.password = password;
    }

    // методы сериализации и десериализации данных
    public override void Deserialize(NetworkReader reader)
    {
        login = reader.ReadString();
        password = reader.ReadString();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(login);
        writer.Write(password);
    }
}