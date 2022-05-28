using Unity.Networking.Transport;
using UnityEngine;

public class NetPlayUntilMark : NetMessage
{
	public NetPlayUntilMark()
	{
		Code = OpCode.PLAY_UNTIL_MARK;
	}

	public NetPlayUntilMark(DataStreamReader reader)
	{
		Code = OpCode.PLAY_UNTIL_MARK;
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
		NetUtility.C_PLAY_UNTIL_MARK?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_PLAY_UNTIL_MARK?.Invoke(this, networkConnection);
	}
}
