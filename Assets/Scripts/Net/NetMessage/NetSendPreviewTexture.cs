using ARKitStream.Internal;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetSendPreviewTexture : NetMessage
{
	public NativeArray<byte> InRawTextureData { get; private set; }
	public NativeArray<byte> OutRawTextureData { get; private set; }
	//public byte[] RawTextureData { get; private set; }
	public int Width { get; private set; }
	public int Height { get; private set; }

	private int _dataLenght;

	public NetSendPreviewTexture(NativeArray<byte> rawTextureData, int width, int heihgt)
	{
		Code = OpCode.SEND_PREVIEW_TEXTURE;
		Width = width;
		Height = heihgt;

		_dataLenght = rawTextureData.Length;
		InRawTextureData = rawTextureData;
	}

	public NetSendPreviewTexture(DataStreamReader reader)
	{
		Code = OpCode.SEND_PREVIEW_TEXTURE;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(Width);
		writer.WriteInt(Height);
		writer.WriteInt(_dataLenght);

		//NativeArray<byte> currentNativeArray = NativeArrayExtension.FromRawBytes<byte>(RawTextureData, Allocator.Temp);
		//InRawTextureData = new NativeArray<byte>(_dataLenght, Allocator.Temp);
		if (writer.WriteBytes(InRawTextureData))
		{
			Debug.Log("Good Write Bytes");
		}
		else
		{
			Debug.Log("No Good Write Bytes");

		}

		if (writer.HasFailedWrites)
		{
			Debug.Log("Failed Writes Preview Texture.");
		}

		Debug.Log($"writer.Length = {writer.Length}");
		
		//currentNativeArray.Dispose();

		//writer.WriteFixedString128(Text);
		//writer.WriteBytes(nativeArray);
		//nativeArray.Dispose();
	}

	public override void Deserialize(DataStreamReader reader)
	{
		Width = reader.ReadInt();
		Height = reader.ReadInt();
		_dataLenght = reader.ReadInt();

		OutRawTextureData = new NativeArray<byte>(_dataLenght, Allocator.Temp);
		//NativeArray<byte> currentNativeArray = new NativeArray<byte>();
		reader.ReadBytes(OutRawTextureData);
		/*
		byte[] currentRawTextureBytes = new byte[currentNativeArray.Length];
		currentRawTextureBytes = NativeArrayExtension.ToRawBytes<byte>(currentNativeArray);

		InRawTextureData = currentRawTextureBytes;
		*/
		//currentNativeArray.Dispose();

		//byte[] screenshot = nativeArray.ToArray();
		//Screenshot.LoadImage(screenshot);
		//Screenshot.LoadRawTextureData();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_SEND_PREVIEW_TEXTURE?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_SEND_PREVIEW_TEXTURE?.Invoke(this, networkConnection);
	}
}