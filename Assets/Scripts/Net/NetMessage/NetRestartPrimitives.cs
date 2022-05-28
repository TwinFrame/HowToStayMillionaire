using Unity.Networking.Transport;

public class NetRestartPrimitives : NetMessage
{
	public NetRestartPrimitives()
	{
		Code = OpCode.RESTART_PRIMITIVES;
	}

	public NetRestartPrimitives(DataStreamReader reader)
	{
		Code = OpCode.RESTART_PRIMITIVES;
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
		NetUtility.C_RESTART_PRIMITIVES?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_RESTART_PRIMITIVES?.Invoke(this, networkConnection);
	}
}

