using Unity.Networking.Transport;

public class NetInteractableNextButton : NetMessage
{
	private int _isInteractableInt;
	public bool IsInteractable { get { if (_isInteractableInt == 0) return false; else return true; } private set { } }

	//public TypesMenu TypesMenu { get; set; }

	public NetInteractableNextButton(bool isInteractable)
	{
		//Code = OpCode.INTERACTABLE_NEXT_BUTTON;

 		if (isInteractable)
			_isInteractableInt = 1;
		else
			_isInteractableInt = 0;
	}

	public NetInteractableNextButton(DataStreamReader reader)
	{
		//Code = OpCode.INTERACTABLE_NEXT_BUTTON;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);
		writer.WriteInt(_isInteractableInt);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		_isInteractableInt = reader.ReadInt();
	}

	public override void RecievedOnClient()
	{
		//NetUtility.C_INTERACTABLE_NEXT_BUTTON?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		//NetUtility.S_INTERACTABLE_NEXT_BUTTON?.Invoke(this, networkConnection);
	}
}

