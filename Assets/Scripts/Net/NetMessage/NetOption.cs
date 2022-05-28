using Unity.Networking.Transport;
using UnityEngine;

public class NetOption : NetMessage
{
	public int Option { get; private set; }

	public NetOption(int option)
	{
		Code = OpCode.OPTION;
		Option = option;
	}

	public NetOption(DataStreamReader reader)
	{
		Code = OpCode.OPTION;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(Option);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		Option = reader.ReadInt();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_OPTION?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_OPTION?.Invoke(this, networkConnection);
	}
}
