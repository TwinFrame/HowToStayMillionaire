using Unity.Networking.Transport;
using UnityEngine;

public class NetUpdateAllAudioChannels : NetMessage
{
	public float NormalizeMaster { get; private set; }
	public float NormalizeFx { get; private set; }
	public float NormalizeCountdown { get; private set; }
	public float NormalizeQuestion { get; private set; }
	public float NormalizeMusic { get; private set; }

	public NetUpdateAllAudioChannels()
	{
		Code = OpCode.UPDATE_ALL_AUDIO_CHANNELS;
	}

	public NetUpdateAllAudioChannels(float normalizeMaster, float normalizeFx, float normalizeCountdown,
		float normalizeQuestion, float normalizeMusic)
	{
		Code = OpCode.UPDATE_ALL_AUDIO_CHANNELS;
		NormalizeMaster = normalizeMaster;
		NormalizeFx = normalizeFx;
		NormalizeCountdown = normalizeCountdown;
		NormalizeQuestion = normalizeQuestion;
		NormalizeMusic = normalizeMusic;
	}

	public NetUpdateAllAudioChannels(DataStreamReader reader)
	{
		Code = OpCode.UPDATE_ALL_AUDIO_CHANNELS;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteFloat(NormalizeMaster);
		writer.WriteFloat(NormalizeFx);
		writer.WriteFloat(NormalizeCountdown);
		writer.WriteFloat(NormalizeQuestion);
		writer.WriteFloat(NormalizeMusic);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		NormalizeMaster = reader.ReadFloat();
		NormalizeFx = reader.ReadFloat();
		NormalizeCountdown = reader.ReadFloat();
		NormalizeQuestion = reader.ReadFloat();
		NormalizeMusic = reader.ReadFloat();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_UPDATE_ALL_AUDIO_CHANNELS?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_UPDATE_ALL_AUDIO_CHANNELS?.Invoke(this, networkConnection);
	}
}
