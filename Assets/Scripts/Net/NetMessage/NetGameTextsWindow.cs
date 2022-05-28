using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetGameTextsWindow : NetMessage
{
	private string _nameOfGame;
	private string _finalText;

	private char[] _monetaryUnits;
	private int _unitsCount;
	private char _currentChar;

	public string NameOfGame => _nameOfGame;
	public string FinalText => _finalText;
	public char[] MonetaryUnits => _monetaryUnits;

	public NetGameTextsWindow(string nameOfGame, string finalText, char[] monetaryUnits)
	{
		Code = OpCode.GAME_TEXTS;
		_nameOfGame = nameOfGame;
		_finalText = finalText;
		_monetaryUnits = monetaryUnits;
	}

	public NetGameTextsWindow(DataStreamReader reader)
	{
		Code = OpCode.GAME_TEXTS;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteFixedString128(_nameOfGame);
		writer.WriteFixedString128(_finalText);

		writer.WriteInt(_monetaryUnits.Length);

		foreach (var unit in _monetaryUnits)
			writer.WriteFixedString32(unit.ToString());
	}

	public override void Deserialize(DataStreamReader reader)
	{
		_nameOfGame = reader.ReadFixedString128().ToString();
		_finalText = reader.ReadFixedString128().ToString();

		_unitsCount = reader.ReadInt();

		_monetaryUnits = new char[_unitsCount];
		for (int i = 0; i < _unitsCount; i++)
		{
			_currentChar = reader.ReadFixedString32().ToString()[0];

			_monetaryUnits[i] = _currentChar;
		}
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_GAME_TEXTS?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_GAME_TEXTS?.Invoke(this, networkConnection);
	}
}
