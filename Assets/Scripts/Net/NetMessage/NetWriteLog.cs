using Unity.Networking.Transport;
using UnityEngine;

public class NetWriteLog : NetMessage
{
	public string LogText { get; private set; }

	public NetWriteLog(string logText)
	{
		Code = OpCode.WRITE_LOG;
		LogText = logText;
	}

	public NetWriteLog(DataStreamReader reader)
	{
		Code = OpCode.WRITE_LOG;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteFixedString128(LogText);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		LogText = reader.ReadFixedString128().ToString();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_WRITE_LOG?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_WRITE_LOG?.Invoke(this, networkConnection);
	}
}
