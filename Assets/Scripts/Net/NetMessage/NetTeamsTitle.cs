using Unity.Networking.Transport;
using UnityEngine;

public class NetTeamsTitle : NetMessage
{
	public NetTeamsTitle()
	{
		Code = OpCode.TEAMS_TITLE;
	}

	public NetTeamsTitle(DataStreamReader reader)
	{
		Code = OpCode.TEAMS_TITLE;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
	}

	public override void Deserialize(DataStreamReader reader)
	{
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_TEAM_TITLE?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_TEAM_TITLE?.Invoke(this, networkConnection);
	}
}
