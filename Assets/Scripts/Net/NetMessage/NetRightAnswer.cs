using Unity.Networking.Transport;
using UnityEngine;

public class NetRightAnswer : NetMessage
{
	public NetRightAnswer()
	{
		Code = OpCode.RIGHT_ANSWER;
	}

	public NetRightAnswer(DataStreamReader reader)
	{
		Code = OpCode.RIGHT_ANSWER;
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
		NetUtility.C_RIGHT_ANSWER?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_RIGHT_ANSWER?.Invoke(this, networkConnection);
	}
}
