using Unity.Networking.Transport;
using UnityEngine;

public class NetStartCountdown : NetMessage
{
	public NetStartCountdown()
	{
		Code = OpCode.START_COUNTDOWN;
	}

	public NetStartCountdown(DataStreamReader reader)
	{
		Code = OpCode.START_COUNTDOWN;
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
		NetUtility.C_START_COUNTDOWN?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_START_COUNTDOWN?.Invoke(this, networkConnection);
	}
}
