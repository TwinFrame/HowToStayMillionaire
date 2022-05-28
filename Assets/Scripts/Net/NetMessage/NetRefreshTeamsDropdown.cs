using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class NetRefreshTeamsDropdown : NetMessage
{
	public List<string> TeamsStringFromServer { get; private set; }
	public int TeamsCount { get; private set; }

	private List<string> _teamsString;

	public NetRefreshTeamsDropdown(List<string> teamsString)
	{
		Code = OpCode.REFRESH_TEAMS_DROPDOWN;
		_teamsString = teamsString;
	}

	public NetRefreshTeamsDropdown(DataStreamReader reader)
	{
		Code = OpCode.REFRESH_TEAMS_DROPDOWN;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(_teamsString.Count);

		for (int i = 0; i < _teamsString.Count; i++)
		{
			writer.WriteFixedString128($"{_teamsString[i]}");
		}

	}

	public override void Deserialize(DataStreamReader reader)
	{
		TeamsCount = reader.ReadInt();

		TeamsStringFromServer = new List<string>(TeamsCount);

		for (int i = 0; i < TeamsCount; i++)
		{
			TeamsStringFromServer.Add(reader.ReadFixedString128().ToString());
		}
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_REFRESH_TEAMS_DROPDOWN?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_REFRESH_TEAMS_DROPDOWN?.Invoke(this, networkConnection);
	}
}
