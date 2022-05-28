using Unity.Networking.Transport;
using UnityEngine;

public class NetForceOnPrimitives : NetMessage
{
	private float _value;
	private int _isInside;

	public float Value => _value;
	public bool IsInside { get { if (_isInside == 0) return false; else return true; } private set { } }

	public NetForceOnPrimitives(float value, bool isInside)
	{
		Code = OpCode.FORCE_ON_PRIMITIVES;
		_value = value;

		if (isInside)
			_isInside = 1;
		else
			_isInside = 0;
	}

	public NetForceOnPrimitives(DataStreamReader reader)
	{
		Code = OpCode.FORCE_ON_PRIMITIVES;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteFloat(_value);
		writer.WriteInt(_isInside);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		_value = reader.ReadFloat();
		_isInside = reader.ReadInt();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_FORCE_ON_PRIMITIVES?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_FORCE_ON_PRIMITIVES?.Invoke(this, networkConnection);
	}
}
