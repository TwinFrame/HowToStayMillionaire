using Unity.Networking.Transport;
using UnityEngine;

public class NetTabInteractables : NetMessage
{
	public TypesOfTab Type { get; private set; }

	private bool[] _isInteractables;

	public bool[] IsInteractables => _isInteractables;


	public NetTabInteractables(TypesOfTab type, bool[] isInteractables)
	{
		Code = OpCode.TAB_INTERACTABLES;

		Type = type;

		_isInteractables = new bool[isInteractables.Length];
		for (int i = 0; i < _isInteractables.Length; i++)
			_isInteractables[i] = isInteractables[i];
	}

	public NetTabInteractables(DataStreamReader reader)
	{
		Code = OpCode.TAB_INTERACTABLES;
		Deserialize(reader);
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte((byte)Code);

		writer.WriteInt(((int)Type));

		writer.WriteInt(_isInteractables.Length);

		foreach (bool interactable in _isInteractables)
		{
			if(interactable)
				writer.WriteInt(1);
			else
				writer.WriteInt(0);
		}
	}

	public override void Deserialize(DataStreamReader reader)
	{
		Type = (TypesOfTab)reader.ReadInt();

		int lenght = reader.ReadInt();

		bool[] currentInteractables = new bool[lenght];

		for (int i = 0; i < currentInteractables.Length; i++)
		{
			int value = reader.ReadInt();

			if(value == 1)
				currentInteractables[i] = true;
			else
				currentInteractables[i] = false;
		}

		_isInteractables = currentInteractables;
	}

	public override void RecievedOnClient()
	{
		NetUtility.C_TAB_INTERACTABLES?.Invoke(this);
	}

	public override void RecievedOnServer(NetworkConnection networkConnection)
	{
		NetUtility.S_TAB_INTERACTABLES?.Invoke(this, networkConnection);
	}
}
