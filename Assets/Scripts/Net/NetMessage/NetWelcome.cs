using Unity.Networking.Transport;
using UnityEngine;

public class NetWelcome : NetMessage
{
	public int AssignedClient { get; set; }

	public NetWelcome()
	{
		Code = OpCode.WELCOME;
	}

	public NetWelcome(DataStreamReader reader)
	{
		Code = OpCode.WELCOME;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(AssignedClient);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		AssignedClient = reader.ReadInt();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_WELCOME?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_WELCOME?.Invoke(this, networkConnection);
	}
}
