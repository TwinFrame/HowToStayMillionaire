using Unity.Networking.Transport;
using UnityEngine;

public class NetMonetaryUnit : NetMessage
{
	public char MonetaryUnit { get; private set; }

	//public char Symbol => _symbol;

	public NetMonetaryUnit(char symbol)
	{
		Code = OpCode.MONETARY_UNIT;
		MonetaryUnit = symbol;
	}

	public NetMonetaryUnit(DataStreamReader reader)
	{
		Code = OpCode.MONETARY_UNIT;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteFixedString32(MonetaryUnit.ToString());
	}

	public override void Deserialize(DataStreamReader reader)
	{
		string symbol = reader.ReadFixedString32().ToString();

		MonetaryUnit = symbol[0];
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_MONETARY_UNIT?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_MONETARY_UNIT?.Invoke(this, networkConnection);
	}
}
