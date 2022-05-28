using Unity.Networking.Transport;
using UnityEngine;

public class NetSetGameDisplayResolution : NetMessage
{
	public int Width { get; private set; }
	public int Height { get; private set; }

	public NetSetGameDisplayResolution(int width, int height)
	{
		Code = OpCode.SET_GAME_RESOLUTION;
		Width = width;
		Height = height;
	}

	public NetSetGameDisplayResolution(DataStreamReader reader)
	{
		Code = OpCode.SET_GAME_RESOLUTION;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(Width);
		writer.WriteInt(Height);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		Width = reader.ReadInt();
		Height = reader.ReadInt();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_SET_GAME_RESOLUTION?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_SET_GAME_RESOLUTION?.Invoke(this, networkConnection);
	}
}
