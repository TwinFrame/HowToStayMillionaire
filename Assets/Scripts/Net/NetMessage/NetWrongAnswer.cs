using Unity.Networking.Transport;
using UnityEngine;

public class NetWrongAnswer : NetMessage
{
	public NetWrongAnswer()
	{
		Code = OpCode.WRONG_ANSWER;
	}

	public NetWrongAnswer(DataStreamReader reader)
	{
		Code = OpCode.WRONG_ANSWER;
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
		NetUtility.C_WRONG_ANSWER?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_WRONG_ANSWER?.Invoke(this, networkConnection);
	}
}
