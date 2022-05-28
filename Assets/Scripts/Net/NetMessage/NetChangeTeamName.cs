using Unity.Networking.Transport;
using UnityEngine;

public class NetChangeTeamName : NetMessage
{
	public int NewNumTeam { get; private set; }
	public string NewNameTeam { get; private set; }

	public NetChangeTeamName(int newNumName, string newNameTeam)
	{
		Code = OpCode.CHANGE_TEAM_NAME;
		NewNumTeam = newNumName;
		NewNameTeam = newNameTeam;
	}

	public NetChangeTeamName(DataStreamReader reader)
	{
		Code = OpCode.CHANGE_TEAM_NAME;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(NewNumTeam);
		writer.WriteFixedString128(NewNameTeam);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		NewNumTeam = reader.ReadInt();
		NewNameTeam = reader.ReadFixedString128().ToString();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_CHANGE_TEAM_NAME?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_CHANGE_TEAM_NAME?.Invoke(this, networkConnection);
	}
}
