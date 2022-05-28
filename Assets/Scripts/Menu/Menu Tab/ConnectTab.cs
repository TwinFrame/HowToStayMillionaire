using UnityEngine;
using UnityEngine.Events;

public class ConnectTab : BaseTab
{
	protected string _localhost = "127.0.0.1";
	protected string _port = "8007";

	public UnityAction BlockHotkeyEvent;
	public UnityAction UnblockHotkeyEvent;

	public override void RefreshTab()
	{
	}

	public override bool[] GetInteractables()
	{
		return new bool[0];
	}

	public override void SetTabInteractables(bool[] isInteractables)
	{
		throw new System.NotImplementedException();
	}

	public void OnBlockHotkey(string text)
	{
		Debug.Log("Text from Event: " + text);
		BlockHotkeyEvent?.Invoke();
	}

	public void OnUnblockHotkey(string text)
	{
		UnblockHotkeyEvent?.Invoke();
	}
}