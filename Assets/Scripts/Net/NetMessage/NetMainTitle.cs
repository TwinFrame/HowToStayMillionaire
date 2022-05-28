using Unity.Networking.Transport;

public class NetMainTitle : NetMessage
{
	public NetMainTitle()
	{
		Code = OpCode.MAIN_TITLE;
	}

	public NetMainTitle(DataStreamReader reader)
	{
		Code = OpCode.MAIN_TITLE;
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
		NetUtility.C_MAIN_TITLE?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_MAIN_TITLE?.Invoke(this, networkConnection);
	}
}

