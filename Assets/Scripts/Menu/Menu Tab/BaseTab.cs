using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseTab : MonoBehaviour
{
	[SerializeField] protected TypesOfTab _typesMenu;

	protected bool IsOpen;

	public TypesOfTab TypesMenuReadOnly => _typesMenu;

	public UnityAction<TypesOfLog, string> OnSendLog;

	public abstract void RefreshTab();
	public abstract bool[] GetInteractables();
	public abstract void SetTabInteractables(bool[] isInteractables);
}
