using Unity.Networking.Transport;
using UnityEngine;

public class NetDoubleClickAudioChannel : NetMessage
{
	public TypesOfAudioChannel Channel { get; private set; }

	public NetDoubleClickAudioChannel(TypesOfAudioChannel channel)
	{
		Code = OpCode.AUDIO_CHANNEL_DOUBLE_CLICKED;
		Channel = channel;
	}

	public NetDoubleClickAudioChannel(DataStreamReader reader)
	{
		Code = OpCode.AUDIO_CHANNEL_DOUBLE_CLICKED;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt((int)Channel);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		Channel = (TypesOfAudioChannel)reader.ReadInt();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_AUDIO_CHANNEL_DOUBLE_CLICKED?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_AUDIO_CHANNEL_DOUBLE_CLICKED?.Invoke(this, networkConnection);
	}
}

