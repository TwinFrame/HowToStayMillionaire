using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetRefreshPalettes : NetMessage
{
	public NetRefreshPalettes()
	{
		Code = OpCode.REFRESH_PALETTES;
	}

	public NetRefreshPalettes(DataStreamReader reader)
	{
		Code = OpCode.REFRESH_PALETTES;
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
		NetUtility.C_REFRESH_PALETTES?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_REFRESH_PALETTES?.Invoke(this, networkConnection);
	}
}