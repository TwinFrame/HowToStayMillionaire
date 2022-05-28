using Unity.Networking.Transport;
using UnityEngine;

public class NetUserDeletedLogo : NetMessage
{
	public NetUserDeletedLogo()
	{
		Code = OpCode.USER_DELETED_LOGO;
	}

	public NetUserDeletedLogo(DataStreamReader reader)
	{
		Code = OpCode.USER_DELETED_LOGO;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
	}

	public override void Deserialize(DataStreamReader reader)
	{
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_USER_DELETED_LOGO?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_USER_DELETED_LOGO?.Invoke(this, networkConnection);
	}
}
