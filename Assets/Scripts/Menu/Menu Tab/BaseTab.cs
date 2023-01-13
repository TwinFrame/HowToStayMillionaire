using UnityEngine;

public abstract class BaseTab : MonoBehaviour
{
	[SerializeField] protected TypesOfTab _typesMenu;

	protected bool IsOpen;

	public TypesOfTab TypesMenuReadOnly => _typesMenu;

	public abstract void RefreshTab();
	public abstract bool[] GetInteractables();
	public abstract void SetTabInteractables(bool[] isInteractables);
}
