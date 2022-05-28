using Unity.Networking.Transport;
using UnityEngine;

public class NetChangedGameTexts : NetMessage
{
	public string Name { get; private set; }
	public string FinalText { get; private set; }

	public NetChangedGameTexts(string name, string finalText)
	{
		Code = OpCode.CHANGED_GAME_TEXT;
		Name = name;
		FinalText = finalText;
	}

	public NetChangedGameTexts(DataStreamReader reader)
	{
		Code = OpCode.CHANGED_GAME_TEXT;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteFixedString128(Name);
		writer.WriteFixedString128(FinalText);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		Name = reader.ReadFixedString128().ToString();
		FinalText = reader.ReadFixedString128().ToString();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_CHANGED_GAME_TEXTS?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_CHANGED_GAME_TEXTS?.Invoke(this, networkConnection);
	}
}

