using UnityEngine.Networking;


public class HashMessage : MessageBase
{
    public NetworkHash128 hash;

    public HashMessage()
    {
    }

    public HashMessage(NetworkHash128 hash)
    {
        this.hash = hash;
    }

    public override void Deserialize(NetworkReader reader)
    {
        hash = reader.ReadNetworkHash128();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(hash);
    }
}