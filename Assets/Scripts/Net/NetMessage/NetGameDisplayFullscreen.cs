using Unity.Networking.Transport;
using UnityEngine;

public class NetGameDisplayFullscreen : NetMessage
{
	private int _isFullscreen;

	public bool IsFullscreen { get { if (_isFullscreen == 0) return false; else return true; } private set { } }

	public NetGameDisplayFullscreen(bool isFullscreen)
	{
		Code = OpCode.GAME_FULLSCREEN;

		if (isFullscreen)
			_isFullscreen = 1;
		else
			_isFullscreen = 0;
	}

	public NetGameDisplayFullscreen(DataStreamReader reader)
	{
		Code = OpCode.GAME_FULLSCREEN;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(_isFullscreen);

	}

	public override void Deserialize(DataStreamReader reader)
	{
		_isFullscreen = reader.ReadInt();	
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_GAME_FULLSCREEN?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_GAME_FULLSCREEN?.Invoke(this, networkConnection);
	}
}
