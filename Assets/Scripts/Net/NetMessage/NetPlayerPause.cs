using Unity.Networking.Transport;
using UnityEngine;

public class NetPlayerPause : NetMessage
{
	public NetPlayerPause()
	{
		Code = OpCode.PLAYER_PAUSE;
	}

	public NetPlayerPause(DataStreamReader reader)
	{
		Code = OpCode.PLAYER_PAUSE;
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
		NetUtility.C_PLAYER_PAUSE?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_PLAYER_PAUSE?.Invoke(this, networkConnection);
	}
}
