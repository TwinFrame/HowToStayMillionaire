using Unity.Networking.Transport;
using UnityEngine;

public class NetMainTitleStateButton : NetMessage
{
	public TypesOfButtonState ButtonState { get; private set; }

	public NetMainTitleStateButton(TypesOfButtonState buttonState)
	{
		Code = OpCode.MAIN_TITLE_STATE_BUTTON;
		ButtonState = buttonState;
	}

	public NetMainTitleStateButton(DataStreamReader reader)
	{
		Code = OpCode.MAIN_TITLE_STATE_BUTTON;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt((int)ButtonState);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		ButtonState = (TypesOfButtonState)reader.ReadInt();
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_MAIN_TITLE_STATE_BUTTON?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_MAIN_TITLE_STATE_BUTTON?.Invoke(this, networkConnection);
	}
}
