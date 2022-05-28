using Unity.Networking.Transport;
using UnityEngine;

public class NetPlayAfterMark : NetMessage
{
	public NetPlayAfterMark()
	{
		Code = OpCode.PLAY_AFTER_MARK;
	}

	public NetPlayAfterMark(DataStreamReader reader)
	{
		Code = OpCode.PLAY_AFTER_MARK;
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
		NetUtility.C_PLAY_AFTER_MARK?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_PLAY_AFTER_MARK?.Invoke(this, networkConnection);
	}
}
