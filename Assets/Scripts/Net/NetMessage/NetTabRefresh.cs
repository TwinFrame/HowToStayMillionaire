using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetTabRefresh : NetMessage
{
	public TypesOfTab Type { get; private set; }

	public NetTabRefresh(TypesOfTab type)
	{
		Code = OpCode.TAB_REFRESH;

		Type = type;
	}

	public NetTabRefresh(DataStreamReader reader)
	{
		Code = OpCode.TAB_REFRESH;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(((int)Type));
	}

	public override void Deserialize(DataStreamReader reader)
	{
		Type = (TypesOfTab)reader.ReadInt();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_TAB_REFRESH?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_TAB_REFRESH?.Invoke(this, networkConnection);
	}
}

