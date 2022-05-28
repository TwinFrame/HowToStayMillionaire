using Unity.Networking.Transport;
using UnityEngine;

public class NetNextTitle : NetMessage
{
	public NetNextTitle()
	{
		Code = OpCode.NEXT_TITLE;
	}

	public NetNextTitle(DataStreamReader reader)
	{
		Code = OpCode.NEXT_TITLE;
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
		NetUtility.C_NEXT_TITLE?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_NEXT_TITLE?.Invoke(this, networkConnection);
	}
}
