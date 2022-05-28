using Unity.Networking.Transport;
using UnityEngine;

public class NetFireworks : NetMessage
{
	public NetFireworks()
	{
		Code = OpCode.FIREWORKS;
	}

	public NetFireworks(DataStreamReader reader)
	{
		Code = OpCode.FIREWORKS;
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
		NetUtility.C_FIREWORKS?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_FIREWORKS?.Invoke(this, networkConnection);
	}
}
