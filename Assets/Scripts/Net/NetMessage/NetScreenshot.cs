using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetScreenshot : NetMessage
{
	public Texture2D Screenshot { get; private set; }
	public string Text { get; private set; }

	public NetScreenshot(Texture2D texture, string text)
	{
		Code = OpCode.SCREENSHOT;
		Screenshot = texture;
		Text = text;
	}

	public NetScreenshot(DataStreamReader reader)
	{
		Code = OpCode.SCREENSHOT;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		byte[] screenshot = Screenshot.EncodeToJPG();
		NativeArray<byte> nativeArray = new NativeArray<byte>(screenshot, Allocator.Temp);
		
		writer.WriteByte((byte)Code);
		writer.WriteFixedString128(Text);
		writer.WriteBytes(nativeArray);
		nativeArray.Dispose();
	}

	public override void Deserialize(DataStreamReader reader)
	{
		Text = reader.ReadFixedString128().ToString();
		
		NativeArray<byte> nativeArray = new NativeArray<byte>();
		reader.ReadBytes(nativeArray);

		byte[] screenshot = nativeArray.ToArray();
		Screenshot.LoadImage(screenshot);
		//Screenshot.LoadRawTextureData();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_SCREENSHOT?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_SCREENSHOT?.Invoke(this, networkConnection);
	}
}
