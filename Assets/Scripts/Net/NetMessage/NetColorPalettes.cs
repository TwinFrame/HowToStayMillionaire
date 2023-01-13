using System;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class NetColorPalettes : NetMessage
{
	private List<ColorPalette> _palettesFromServer;
	private int _palettesCount;

	public List<ColorPalette> PalettesToClient { get; private set; }
	public int CurrentNumPalette { get; private set; }
	public GameObject Parent { get; private set; }

	public NetColorPalettes(List<ColorPalette> palettes, int currentNumPalette)
	{
		Code = OpCode.COLOR_PALETTES;
		_palettesFromServer = palettes;
		CurrentNumPalette = currentNumPalette;
	}

	public NetColorPalettes(DataStreamReader reader)
	{
		Code = OpCode.COLOR_PALETTES;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(CurrentNumPalette);
		writer.WriteInt(_palettesFromServer.Count);
		
		for (int i = 0; i < _palettesFromServer.Count; i++)
		{
			writer.WriteFloat(_palettesFromServer[i].MainColor.r);
			writer.WriteFloat(_palettesFromServer[i].MainColor.g);
			writer.WriteFloat(_palettesFromServer[i].MainColor.b);
			writer.WriteFloat(_palettesFromServer[i].MainColor.a);

			writer.WriteFloat(_palettesFromServer[i].SlaveColor.r);
			writer.WriteFloat(_palettesFromServer[i].SlaveColor.g);
			writer.WriteFloat(_palettesFromServer[i].SlaveColor.b);
			writer.WriteFloat(_palettesFromServer[i].SlaveColor.a);

			writer.WriteFloat(_palettesFromServer[i].Add1Color.r);
			writer.WriteFloat(_palettesFromServer[i].Add1Color.g);
			writer.WriteFloat(_palettesFromServer[i].Add1Color.b);
			writer.WriteFloat(_palettesFromServer[i].Add1Color.a);

			writer.WriteFloat(_palettesFromServer[i].Add2Color.r);
			writer.WriteFloat(_palettesFromServer[i].Add2Color.g);
			writer.WriteFloat(_palettesFromServer[i].Add2Color.b);
			writer.WriteFloat(_palettesFromServer[i].Add2Color.a);

			writer.WriteFloat(_palettesFromServer[i].TextColor.r);
			writer.WriteFloat(_palettesFromServer[i].TextColor.g);
			writer.WriteFloat(_palettesFromServer[i].TextColor.b);
			writer.WriteFloat(_palettesFromServer[i].TextColor.a);

			writer.WriteFloat(_palettesFromServer[i].SelectedColor.r);
			writer.WriteFloat(_palettesFromServer[i].SelectedColor.g);
			writer.WriteFloat(_palettesFromServer[i].SelectedColor.b);
			writer.WriteFloat(_palettesFromServer[i].SelectedColor.a);

			writer.WriteFloat(_palettesFromServer[i].RightColor.r);
			writer.WriteFloat(_palettesFromServer[i].RightColor.g);
			writer.WriteFloat(_palettesFromServer[i].RightColor.b);
			writer.WriteFloat(_palettesFromServer[i].RightColor.a);

			writer.WriteFloat(_palettesFromServer[i].WrongColor.r);
			writer.WriteFloat(_palettesFromServer[i].WrongColor.g);
			writer.WriteFloat(_palettesFromServer[i].WrongColor.b);
			writer.WriteFloat(_palettesFromServer[i].WrongColor.a);

			writer.WriteFixedString128(_palettesFromServer[i].Name);

			writer.WriteInt(Convert.ToInt32(_palettesFromServer[i].IsUserAllowedChange));
		}
	}

	public override void Deserialize(DataStreamReader reader)
	{
		PalettesToClient = new List<ColorPalette>(_palettesCount);
		Parent = new GameObject("ColorPalettes");

		CurrentNumPalette = reader.ReadInt();

		_palettesCount = reader.ReadInt();

		for (int i = 0; i < _palettesCount; i++)
		{
			float r_MainColor = reader.ReadFloat();
			float g_MainColor = reader.ReadFloat();
			float b_MainColor = reader.ReadFloat();
			float a_MainColor = reader.ReadFloat();
			Color mainColor = new Color(r_MainColor, g_MainColor, b_MainColor, a_MainColor);

			float r_SlaveColor = reader.ReadFloat();
			float g_SlaveColor = reader.ReadFloat();
			float b_SlaveColor = reader.ReadFloat();
			float a_SlaveColor = reader.ReadFloat();
			Color slaveColor = new Color(r_SlaveColor, g_SlaveColor, b_SlaveColor, a_SlaveColor);

			float r_Add1Color = reader.ReadFloat();
			float g_Add1Color = reader.ReadFloat();
			float b_Add1Color = reader.ReadFloat();
			float a_Add1Color = reader.ReadFloat();
			Color add1Color = new Color(r_Add1Color, g_Add1Color, b_Add1Color, a_Add1Color);

			float r_Add2Color = reader.ReadFloat();
			float g_Add2Color = reader.ReadFloat();
			float b_Add2Color = reader.ReadFloat();
			float a_Add2Color = reader.ReadFloat();
			Color add2Color = new Color(r_Add2Color, g_Add2Color, b_Add2Color, a_Add2Color);

			float r_TextColor = reader.ReadFloat();
			float g_TextColor = reader.ReadFloat();
			float b_TextColor = reader.ReadFloat();
			float a_TextColor = reader.ReadFloat();
			Color textColor = new Color(r_TextColor, g_TextColor, b_TextColor, a_TextColor);

			float r_SelectedColor = reader.ReadFloat();
			float g_SelectedColor = reader.ReadFloat();
			float b_SelectedColor = reader.ReadFloat();
			float a_SelectedColor = reader.ReadFloat();
			Color selectedColor = new Color(r_SelectedColor, g_SelectedColor, b_SelectedColor, a_SelectedColor);

			float r_RightColor = reader.ReadFloat();
			float g_RightColor = reader.ReadFloat();
			float b_RightColor = reader.ReadFloat();
			float a_RightColor = reader.ReadFloat();
			Color rightColor = new Color(r_RightColor, g_RightColor, b_RightColor, a_RightColor);

			float r_WrongColor = reader.ReadFloat();
			float g_WrongColor = reader.ReadFloat();
			float b_WrongColor = reader.ReadFloat();
			float a_WrongColor = reader.ReadFloat();
			Color wrongColor = new Color(r_WrongColor, g_WrongColor, b_WrongColor, a_WrongColor);

			string currentPaletteName = reader.ReadFixedString128().ToString();

			bool isUserAllowedChange = Convert.ToBoolean(reader.ReadInt());

			ColorPalette colorPalette = Parent.AddComponent<ColorPalette>();
			colorPalette.Set(currentPaletteName, mainColor, slaveColor, add1Color,
				add2Color, textColor, selectedColor, rightColor, wrongColor, isUserAllowedChange);

			PalettesToClient.Add(colorPalette);
		}
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_COLOR_PALETTES?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_COLOR_PALETTES?.Invoke(this, networkConnection);
	}
}
