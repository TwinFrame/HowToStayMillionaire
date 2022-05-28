using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class NetChangeColorInPalette : NetMessage
{

	private readonly int _numPalette;
	private readonly TypesOfGameColor _gameTransferColor;
	private readonly Color _transferColor;

	public int NumPalette { get; private set; }
	public TypesOfGameColor GameTransferColor { get; private set; }
	public Color TransferColor { get; private set; }

	public NetChangeColorInPalette(int numPalette, TypesOfGameColor gameColor, Color color)
	{
		Code = OpCode.CHANGE_COLOR_IN_PALETTE;
		_numPalette = numPalette;
		_gameTransferColor = gameColor;
		_transferColor = color;
	}

	public NetChangeColorInPalette(DataStreamReader reader)
	{
		Code = OpCode.CHANGE_COLOR_IN_PALETTE;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(_numPalette);
		writer.WriteInt(((int)_gameTransferColor));
		writer.WriteFloat(_transferColor.r);
		writer.WriteFloat(_transferColor.g);
		writer.WriteFloat(_transferColor.b);
		writer.WriteFloat(_transferColor.a);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		NumPalette = reader.ReadInt();

		GameTransferColor = (TypesOfGameColor)reader.ReadInt();

		float r = reader.ReadFloat(); 
		float g = reader.ReadFloat(); 
		float b = reader.ReadFloat(); 
		float a = reader.ReadFloat();

		TransferColor = new Color(r, g, b, a);
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_CHANGE_COLOR_IN_PALETTE?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_CHANGE_COLOR_IN_PALETTE?.Invoke(this, networkConnection);
	}
}
