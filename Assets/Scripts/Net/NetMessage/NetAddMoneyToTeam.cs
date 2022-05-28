using Unity.Networking.Transport;
using UnityEngine;

public class NetAddMoneyToTeam : NetMessage
{
	public int NumTeam { get; private set; }
	public string Money { get; private set; }

	public NetAddMoneyToTeam(int newNumName, string money)
	{
		Code = OpCode.ADD_MONEY_TO_TEAM;
		NumTeam = newNumName;
		Money = money;
	}

	public NetAddMoneyToTeam(DataStreamReader reader)
	{
		Code = OpCode.ADD_MONEY_TO_TEAM;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(NumTeam);
		writer.WriteFixedString128(Money);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		NumTeam = reader.ReadInt();
		Money = reader.ReadFixedString128().ToString();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_ADD_MONEY_TO_TEAM?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_ADD_MONEY_TO_TEAM?.Invoke(this, networkConnection);
	}
}
