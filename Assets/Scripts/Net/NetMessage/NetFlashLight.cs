using Unity.Networking.Transport;
using UnityEngine;

public class NetFlashLight : NetMessage
{
	public NetFlashLight()
	{
		Code = OpCode.FLASH_LIGHT;
	}

	public NetFlashLight(DataStreamReader reader)
	{
		Code = OpCode.FLASH_LIGHT;
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
		NetUtility.C_FLASH_LIGHT?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_FLASH_LIGHT?.Invoke(this, networkConnection);
	}
}
