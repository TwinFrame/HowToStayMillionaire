using Unity.Networking.Transport;
using UnityEngine;

public class NetUserLoadedLogo : NetMessage
{
	public string Path { get; private set; }

	/*
	public NetUserLoadedLogo()
	{
		Code = OpCode.USER_LOADED_LOGO;
	}
	*/

	public NetUserLoadedLogo(string path)
	{
		Code = OpCode.USER_LOADED_LOGO;
		Path = path;
	}

	public NetUserLoadedLogo(DataStreamReader reader)
	{
		Code = OpCode.USER_LOADED_LOGO;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteFixedString128(Path);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		Path = reader.ReadFixedString128().ToString();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_USER_LOADED_LOGO?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_USER_LOADED_LOGO?.Invoke(this, networkConnection);
	}
}
