using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuGameTextsWindows : MenuWindow
{
	[SerializeField] private Button _quitButton;
	[SerializeField] private TMP_Dropdown _monetaryUnitsDropdown;
	[SerializeField] private TMP_InputField _nameInputField;
	[SerializeField] private TMP_InputField _finalTextInputField;
	[SerializeField] private Button _setButton;

	public UnityAction OnCloseWindowEvent;
	public UnityAction<char> OnChangeCurrentSymbolEvent;
	public UnityAction<string, string> OnChangedGameTextsEvent;

	public UnityAction OnBlockHotkeyEvent;
	public UnityAction OnUnblockHotkeyEvent;

	private readonly List<TMP_Dropdown.OptionData> _currentMonetaryUnits = new List<TMP_Dropdown.OptionData>();

	private void OnEnable()
	{
		_quitButton.onClick.AddListener(CloseWindow);
		_monetaryUnitsDropdown.onValueChanged.AddListener(OnChangeCurrentSymbol);
		_setButton.onClick.AddListener(SetGameTexts);

		_nameInputField.onSelect.AddListener(OnBlockHotkey);
		_nameInputField.onDeselect.AddListener(OnUnblockHotkey);
		_finalTextInputField.onSelect.AddListener(OnBlockHotkey);
		_finalTextInputField.onDeselect.AddListener(OnUnblockHotkey);
	}

	private void OnDisable()
	{
		_quitButton.onClick.RemoveListener(CloseWindow);
		_monetaryUnitsDropdown.onValueChanged.RemoveListener(OnChangeCurrentSymbol);
		_setButton.onClick.RemoveListener(SetGameTexts);

		_nameInputField.onSelect.RemoveListener(OnBlockHotkey);
		_nameInputField.onDeselect.RemoveListener(OnUnblockHotkey);
		_finalTextInputField.onSelect.RemoveListener(OnBlockHotkey);
		_finalTextInputField.onDeselect.RemoveListener(OnUnblockHotkey);
	}

	public void SetGameTexts(string name, string finalText, char[] monetaryUnits)
	{
		_nameInputField.text = name;
		_finalTextInputField.text = finalText;

		_currentMonetaryUnits.Clear();

		foreach (var unit in monetaryUnits)
			_currentMonetaryUnits.Add(new TMP_Dropdown.OptionData(unit.ToString()));

		_monetaryUnitsDropdown.ClearOptions();
		_monetaryUnitsDropdown.AddOptions(_currentMonetaryUnits);
	}

	private void OnChangeCurrentSymbol(int numSymbol)
	{
		char[] symbolChar = _monetaryUnitsDropdown.options[numSymbol].text.ToCharArray();

		OnChangeCurrentSymbolEvent?.Invoke(symbolChar[0]);
	}

	private void CloseWindow()
	{
		OnCloseWindowEvent?.Invoke();
	}

	private void SetGameTexts()
	{
		OnChangedGameTextsEvent?.Invoke(_nameInputField.text, _finalTextInputField.text);
	}

	public void OnBlockHotkey(string text)
	{
		OnBlockHotkeyEvent?.Invoke();
	}

	public void OnUnblockHotkey(string text)
	{
		OnUnblockHotkeyEvent?.Invoke();
	}
}
