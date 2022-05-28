using Unity.Networking.Transport;

public class NetInteractableReplaceCurrentTeam : NetMessage
{
	private int _isInteractableInt;

	public bool IsInteractable { get { if (_isInteractableInt == 0) return false; else return true; } private set { } }

	public NetInteractableReplaceCurrentTeam(bool isInteractable)
	{
		Code = OpCode.INTERACTABLE_REPLACE_CURRENT_TEAM;

		if (isInteractable)
			_isInteractableInt = 1;
		else
			_isInteractableInt = 0;
	}

	public NetInteractableReplaceCurrentTeam(DataStreamReader reader)
	{
		Code = OpCode.INTERACTABLE_REPLACE_CURRENT_TEAM;
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
		NetUtility.C_INTERACTABLE_REPLACE_CURRENT_TEAM?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_INTERACTABLE_REPLACE_CURRENT_TEAM?.Invoke(this, networkConnection);
	}
}

