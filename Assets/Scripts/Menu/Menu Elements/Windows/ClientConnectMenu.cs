using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClientConnectMenu : BaseMenuElement
{
	[Header("Client")]
	[SerializeField] private Button _connectButton;
	[Space]
	[SerializeField] private TMP_InputField _ipInputField;
	[SerializeField] private Button _hintIPButton;
	[Space]
	[SerializeField] private TMP_InputField _portInputField;
	[SerializeField] private Button _hintPortButton;

	public UnityAction<string, string> OnClickClientButtonEvent;
	public UnityAction OnBlockHotkeyEvent;
	public UnityAction OnUnblockHotkeyEvent;

	private string _ipAddress = "127.0.0.1";
	private int _port = 8007;

	private void Awake()
	{
		_hintIPButton.GetComponentInChildren<TMP_Text>().text = _ipAddress.ToString();
		_hintPortButton.GetComponentInChildren<TMP_Text>().text = _port.ToString();
	}

	private void OnEnable()
	{
		_connectButton.onClick.AddListener(OnClickClientButton);
		_hintIPButton.onClick.AddListener(OnHintIPButton);
		_hintPortButton.onClick.AddListener(OnHintPortButton);

		_ipInputField.onSelect.AddListener(OnBlockHotkey);
		_ipInputField.onDeselect.AddListener(OnUnblockHotkey);
		_portInputField.onSelect.AddListener(OnBlockHotkey);
		_portInputField.onDeselect.AddListener(OnUnblockHotkey);
	}

	private void OnDisable()
	{
		_connectButton.onClick.RemoveListener(OnClickClientButton);
		_hintIPButton.onClick.RemoveListener(OnHintIPButton);
		_hintPortButton.onClick.RemoveListener(OnHintPortButton);

		_ipInputField.onSelect.RemoveListener(OnBlockHotkey);
		_ipInputField.onDeselect.RemoveListener(OnUnblockHotkey);
		_portInputField.onSelect.RemoveListener(OnBlockHotkey);
		_portInputField.onDeselect.RemoveListener(OnUnblockHotkey);
	}

	public string GetCurrentIP()
	{
		return _ipInputField.text.ToString();
	}

	public string GetCurrentPort()
	{
		return _portInputField.text.ToString();
	}

	public void EnterHintData()
	{
		OnHintIPButton();
		OnHintPortButton();
	}

	public void SetSpriteOnConnectButton(Sprite sprite)
	{
		_connectButton.image.sprite = sprite;
	}

	private void OnClickClientButton()
	{
		//OnClickClientButtonEvent?.Invoke(_portInputField.text);
	}

	private void OnBlockHotkey(string text)
	{
		OnBlockHotkeyEvent?.Invoke();
	}

	private void OnUnblockHotkey(string text)
	{
		OnUnblockHotkeyEvent?.Invoke();
	}

	private void OnHintIPButton()
	{
		_ipInputField.SetTextWithoutNotify(_ipAddress);
	}

	private void OnHintPortButton()
	{
		_portInputField.SetTextWithoutNotify(_port.ToString());
	}
}
