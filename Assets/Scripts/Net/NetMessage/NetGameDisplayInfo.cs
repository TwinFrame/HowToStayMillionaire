using Unity.Networking.Transport;
using UnityEngine;

public class NetGameDisplayInfo : NetMessage
{
	private int _isFullscreen;

	public string GameDisplayInfo { get; private set; }

	public bool IsFullscreen { get { if (_isFullscreen == 0) return false; else return true; } private set { } }

	public NetGameDisplayInfo(string gameDisplayInfo, bool isFullscreen)
	{
		Code = OpCode.GAME_DISPLAY_INFO;
		GameDisplayInfo = gameDisplayInfo;

		if (isFullscreen)
			_isFullscreen = 1;
		else
			_isFullscreen = 0;
	}

	public NetGameDisplayInfo(DataStreamReader reader)
	{
		Code = OpCode.GAME_DISPLAY_INFO;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteFixedString128(GameDisplayInfo);
		writer.WriteInt(_isFullscreen);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		GameDisplayInfo = reader.ReadFixedString128().ToString();
		_isFullscreen = reader.ReadInt();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_GAME_DISPLAY_INFO?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_GAME_DISPLAY_INFO?.Invoke(this, networkConnection);
	}
}
