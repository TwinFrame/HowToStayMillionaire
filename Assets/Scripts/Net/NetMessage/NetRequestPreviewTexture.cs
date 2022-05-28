using Unity.Networking.Transport;
using UnityEngine;

public class NetRequestPreviewTexture : NetMessage
{
	public int Width { get; private set; }
	public int Height { get; private set; }

	public NetRequestPreviewTexture(int width, int height)
	{
		Code = OpCode.REQUEST_PREVIEW_TEXTURE;
		Width = width;
		Height = height;
	}

	public NetRequestPreviewTexture(DataStreamReader reader)
	{
		Code = OpCode.REQUEST_PREVIEW_TEXTURE;
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
		NetUtility.C_REQUEST_PREVIEW_TEXTURE?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_REQUEST_PREVIEW_TEXTURE?.Invoke(this, networkConnection);
	}
}