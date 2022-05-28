using Unity.Networking.Transport;
using UnityEngine;

public class NetSetPauseMark : NetMessage
{
	public string PauseMark { get; private set; }

	public NetSetPauseMark(string pauseMark)
	{
		Code = OpCode.SET_PAUSE_MARK;
		PauseMark = pauseMark;
	}

	public NetSetPauseMark(DataStreamReader reader)
	{
		Code = OpCode.SET_PAUSE_MARK;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteFixedString32(PauseMark);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		PauseMark = reader.ReadFixedString32().ToString();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_SET_PAUSE_MARK?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_SET_PAUSE_MARK?.Invoke(this, networkConnection);
	}
}
