using Unity.Networking.Transport;
using UnityEngine;

public class NetReplaceCurrentTeam : NetMessage
{
	public int NewNumTeam { get; private set; }

	public NetReplaceCurrentTeam(int newNumName)
	{
		Code = OpCode.REPLACE_CURRENT_TEAM;
		NewNumTeam = newNumName;
	}

	public NetReplaceCurrentTeam(DataStreamReader reader)
	{
		Code = OpCode.REPLACE_CURRENT_TEAM;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(NewNumTeam);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		NewNumTeam = reader.ReadInt();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_REPLACE_CURRENT_TEAM?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_REPLACE_CURRENT_TEAM?.Invoke(this, networkConnection);
	}
}
