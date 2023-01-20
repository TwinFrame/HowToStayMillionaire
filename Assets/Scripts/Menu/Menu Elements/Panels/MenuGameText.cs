using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuGameText : BaseMenuElement
{
	[SerializeField] private TMP_InputField _nameOfGame;
	[SerializeField] private TMP_InputField _finalText;
	[SerializeField] private Button _setTextButton;

	public UnityAction<string, string> ChangedGameTextEvent;
	public UnityAction BlockedHotkeyEvent;
	public UnityAction UnblockedHotkeyEvent;

	private void OnEnable()
	{
		_nameOfGame.onSelect.AddListener(OnBlockHotkey);
		_nameOfGame.onDeselect.AddListener(OnUnblockHotkey);
		_finalText.onSelect.AddListener(OnBlockHotkey);
		_finalText.onDeselect.AddListener(OnUnblockHotkey);
		_setTextButton.onClick.AddListener(ChangeGameText);
	}

	private void OnDisable()
	{
		_nameOfGame.onSelect.RemoveListener(OnBlockHotkey);
		_nameOfGame.onDeselect.RemoveListener(OnUnblockHotkey);
		_finalText.onSelect.RemoveListener(OnBlockHotkey);
		_finalText.onDeselect.RemoveListener(OnUnblockHotkey);
		_setTextButton.onClick.RemoveListener(ChangeGameText);
	}

	public void SetGameText(string name, string finalText)
	{
		_nameOfGame.SetTextWithoutNotify(name);
		_finalText.SetTextWithoutNotify(finalText);
	}

	private void ChangeGameText()
	{
		ChangedGameTextEvent?.Invoke(_nameOfGame.text, _finalText.text);
	}

	private void OnBlockHotkey(string text)
	{
		BlockedHotkeyEvent?.Invoke();
	}

	private void OnUnblockHotkey(string text)
	{
		UnblockedHotkeyEvent?.Invoke();
	}
}
