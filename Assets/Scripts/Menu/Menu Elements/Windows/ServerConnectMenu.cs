using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ServerConnectMenu : MenuPanel
{
	[SerializeField] private Button _connectButton;
	[SerializeField] private TMP_InputField _portInputField;
	[SerializeField] private Button _hintPortButton;

	public UnityAction<string> ClickedServerButtonEvent;
	public UnityAction BlockedHotkeyEvent;
	public UnityAction UnblockedHotkeyEvent;

	private int _port = 8007;

	private void Awake()
	{
		_hintPortButton.GetComponentInChildren<TMP_Text>().text = _port.ToString();
	}

	private void OnEnable()
	{
		_connectButton.onClick.AddListener(OnClickServerButton);
		_hintPortButton.onClick.AddListener(OnHintPortButton);

		_portInputField.onSelect.AddListener(OnBlockHotkey);
		_portInputField.onDeselect.AddListener(OnUnblockHotkey);
	}

	private void OnDisable()
	{
		_connectButton.onClick.RemoveListener(OnClickServerButton);
		_hintPortButton.onClick.RemoveListener(OnHintPortButton);

		_portInputField.onSelect.RemoveListener(OnBlockHotkey);
		_portInputField.onDeselect.RemoveListener(OnUnblockHotkey);
	}

	public string GetCurrentPort()
	{
		return _portInputField.text.ToString();
	}

	public void EnterHintData()
	{
		OnHintPortButton();
	}

	public void SetSpriteOnConnectButton(Sprite sprite)
	{
		_connectButton.image.sprite = sprite;
	}

	private void OnClickServerButton()
	{
		//OnClickServerButtonEvent?.Invoke(_portInputField.text);
	}

	private void OnBlockHotkey(string text)
	{
		BlockedHotkeyEvent?.Invoke();
	}

	private void OnUnblockHotkey(string text)
	{
		UnblockedHotkeyEvent?.Invoke();
	}

	private void OnHintPortButton()
	{
		_portInputField.SetTextWithoutNotify(_port.ToString());
	}
}
