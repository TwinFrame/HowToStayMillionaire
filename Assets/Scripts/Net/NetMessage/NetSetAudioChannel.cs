using Unity.Networking.Transport;
using UnityEngine;

public class NetSetAudioChannel : NetMessage
{
	public TypesOfAudioChannel Channel {get; private set;}

	public float NormalizeVolume { get; private set; }

	public NetSetAudioChannel(TypesOfAudioChannel channel, float normalizeVolume)
	{
		Code = OpCode.SET_AUDIO_CHANNEL;
		Channel = channel;
		NormalizeVolume = normalizeVolume;
	}

	public NetSetAudioChannel(DataStreamReader reader)
	{
		Code = OpCode.SET_AUDIO_CHANNEL;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt((int)Channel);
		writer.WriteFloat(NormalizeVolume);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		Channel = (TypesOfAudioChannel)reader.ReadInt();
		NormalizeVolume = reader.ReadFloat();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_SET_AUDIO_CHANNEL?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_SET_AUDIO_CHANNEL?.Invoke(this, networkConnection);
	}
}
