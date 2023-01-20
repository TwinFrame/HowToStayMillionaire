using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TeamsTab : BaseTab
{
	[Space]
	[Header("Replace Current Team")]
	[SerializeField] private Button _replaceButton;
	[SerializeField] private TMP_Dropdown _replaceTeamDropdown;
	[Header("Change Name")]
	[SerializeField] private Button _changeNameButton;
	[SerializeField] private TMP_InputField _changeNameField;
	[SerializeField] private TMP_Dropdown _changeNameDropdown;
	[Header("Add Money")]
	[SerializeField] private Button _addMoneyButton;
	[SerializeField] private TMP_InputField _addMoneyField;
	[SerializeField] private TMP_Dropdown _addMoneyTeamsDropdown;

	public UnityAction<int> ReplacedCurrentTeamEvent;
	public UnityAction<int, string> ChangedTeamNameEvent;
	public UnityAction<int, string> AddedMoneyEvent;
	public UnityAction BlockedHotkeyEvent;
	public UnityAction UnblockedHotkeyEvent;

	private void OnEnable()
	{
		_replaceButton.onClick.AddListener(ReplaceCurrentTeam);

		_changeNameButton.onClick.AddListener(ChangeTeamName);
		_changeNameField.onSelect.AddListener(OnBlockHotkey);
		_changeNameField.onDeselect.AddListener(OnUnblockHotkey);

		_addMoneyButton.onClick.AddListener(AddMoneyToTeam);
		_addMoneyField.onSelect.AddListener(OnBlockHotkey);
		_addMoneyField.onDeselect.AddListener(OnUnblockHotkey);
	}

	private void OnDisable()
	{
		_replaceButton.onClick.RemoveListener(ReplaceCurrentTeam);

		_changeNameButton.onClick.RemoveListener(ChangeTeamName);
		_changeNameField.onSelect.RemoveListener(OnBlockHotkey);
		_changeNameField.onDeselect.RemoveListener(OnUnblockHotkey);

		_addMoneyButton.onClick.RemoveListener(AddMoneyToTeam);
		_addMoneyField.onSelect.RemoveListener(OnBlockHotkey);
		_addMoneyField.onDeselect.RemoveListener(OnUnblockHotkey);
	}

	public override void RefreshTab()
	{
	}

	public override bool[] GetInteractables()
	{
		return new bool[]
		{
			_replaceButton.interactable,
			_changeNameButton.interactable,
			_addMoneyButton.interactable
		};
	}

	public override void SetTabInteractables(bool[] isInteractables)
	{
		_replaceButton.interactable = isInteractables[0];
		_replaceTeamDropdown.interactable = isInteractables[0];

		_changeNameButton.interactable = isInteractables[1];
		_changeNameDropdown.interactable = isInteractables[1];
		_changeNameField.interactable = isInteractables[1];

		_addMoneyButton.interactable = isInteractables[2];
		_addMoneyField.interactable = isInteractables[2];
		_addMoneyTeamsDropdown.interactable = isInteractables[2];
	}

	public void RefreshTeamsDropdown(List<string> teams)
	{
		_changeNameField.text = string.Empty;
		_addMoneyField.text = string.Empty;

		_replaceTeamDropdown.ClearOptions();
		_replaceTeamDropdown.AddOptions(teams);

		_changeNameDropdown.ClearOptions();
		_changeNameDropdown.AddOptions(teams);

		_addMoneyTeamsDropdown.ClearOptions();
		_addMoneyTeamsDropdown.AddOptions(teams);
	}

	private void ReplaceCurrentTeam()
	{
		OnReplaceCurrentTeam(_replaceTeamDropdown.value);
	}

	public void OnReplaceCurrentTeam(int newNumTeam)
	{
		ReplacedCurrentTeamEvent?.Invoke(newNumTeam);
	}

	private void ChangeTeamName()
	{
		OnChangeTeamName(_changeNameDropdown.value, _changeNameField.text);
	}

	public void OnChangeTeamName(int numTeam, string newName)
	{
		ChangedTeamNameEvent?.Invoke(numTeam, newName);
	}

	private void AddMoneyToTeam()
	{
		OnAddMoneyToTeam(_addMoneyTeamsDropdown.value, _addMoneyField.text);
	}

	public void OnAddMoneyToTeam(int numTeam, string newMoney)
	{
		AddedMoneyEvent?.Invoke(numTeam, newMoney);
	}

	public void OnBlockHotkey(string text)
	{
		BlockedHotkeyEvent?.Invoke();
	}

	public void OnUnblockHotkey(string text)
	{
		UnblockedHotkeyEvent?.Invoke();
	}

	public void InteractableReplaceCurrentTeam(bool isInteractable)
	{
		if (isInteractable)
		{
			_replaceButton.interactable = true;
			_replaceTeamDropdown.interactable = true;
		}
		else
		{
			_replaceTeamDropdown.interactable = false;
			_replaceButton.interactable = false;
		}
	}

	public void InteractableAddMoney(bool isInteractable)
	{
		if (isInteractable)
		{
			_addMoneyButton.interactable = true;
			_addMoneyField.interactable = true;
			_addMoneyTeamsDropdown.interactable = true;
		}
		else
		{
			_addMoneyButton.interactable = false;
			_addMoneyField.interactable = false;
			_addMoneyTeamsDropdown.interactable = false;
		}
	}

	public void InteractableChangeName(bool isInteractable)
	{
		if (isInteractable)
		{
			_changeNameButton.interactable = true;
			_changeNameDropdown.interactable = true;
			_changeNameField.interactable = true;
		}
		else
		{
			_changeNameButton.interactable = false;
			_changeNameDropdown.interactable = false;
			_changeNameField.interactable = false;
		}
	}
}
