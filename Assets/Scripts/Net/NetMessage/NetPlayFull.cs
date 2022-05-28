using Unity.Networking.Transport;
using UnityEngine;

public class NetPlayFull : NetMessage
{
	public NetPlayFull()
	{
		Code = OpCode.PLAY_FULL;
	}

	public NetPlayFull(DataStreamReader reader)
	{
		Code = OpCode.PLAY_FULL;
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
		NetUtility.C_PLAY_FULL?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_PLAY_FULL?.Invoke(this, networkConnection);
	}
}
