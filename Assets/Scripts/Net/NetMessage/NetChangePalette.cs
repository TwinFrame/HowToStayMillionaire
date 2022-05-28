using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class NetChangePalette : NetMessage
{
	private readonly int _numPalette;

	public int NumPalette { get; private set; }

	public NetChangePalette(int numPalette)
	{
		Code = OpCode.CHANGE_PALETTE;
		_numPalette = numPalette;
	}

	public NetChangePalette(DataStreamReader reader)
	{
		Code = OpCode.CHANGE_PALETTE;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(_numPalette);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		NumPalette = reader.ReadInt();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_CHANGE_PALETTE?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_CHANGE_PALETTE?.Invoke(this, networkConnection);
	}
}
