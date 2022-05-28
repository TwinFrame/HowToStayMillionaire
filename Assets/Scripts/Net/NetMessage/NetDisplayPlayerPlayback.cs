using Unity.Networking.Transport;
using UnityEngine;

public class NetDisplayPlayerPlayback : NetMessage
{
	public string Time { get; private set; }

	public NetDisplayPlayerPlayback(string time)
	{
		Code = OpCode.DISPLAY_PLAYER_PLAYBACK;
		Time = time;
	}

	public NetDisplayPlayerPlayback(DataStreamReader reader)
	{
		Code = OpCode.DISPLAY_PLAYER_PLAYBACK;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteFixedString32(Time);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		Time = reader.ReadFixedString32().ToString();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_DISPLAY_PLAYER_PLAYBACK?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_DISPLAY_PLAYER_PLAYBACK?.Invoke(this, networkConnection);
	}
}
