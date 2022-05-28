using Unity.Networking.Transport;
using UnityEngine;

public class NetPlayerLoop : NetMessage
{
	private int _isOnInt;
	public bool IsOnBool { get { if (_isOnInt == 0) return false; else return true; } private set { } }

	public NetPlayerLoop()
	{
		Code = OpCode.PLAYER_LOOP;
	}

	public NetPlayerLoop(bool isInteractable)
	{
		Code = OpCode.PLAYER_LOOP;

		if (isInteractable)
			_isOnInt = 1;
		else
			_isOnInt = 0;
	}

	public NetPlayerLoop(DataStreamReader reader)
	{
		Code = OpCode.PLAYER_LOOP;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt((int)_isOnInt);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		_isOnInt = reader.ReadInt();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_PLAYER_LOOP?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_PLAYER_LOOP?.Invoke(this, networkConnection);
	}
}