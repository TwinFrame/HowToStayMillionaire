using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetRefreshLogo : NetMessage
{
	public NetRefreshLogo()
	{
		Code = OpCode.REFRESH_LOGO;
	}

	public NetRefreshLogo(DataStreamReader reader)
	{
		Code = OpCode.REFRESH_LOGO;
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
		NetUtility.C_REFRESH_LOGO?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_REFRESH_LOGO?.Invoke(this, networkConnection);
	}
}
