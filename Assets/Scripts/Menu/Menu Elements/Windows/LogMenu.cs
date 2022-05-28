using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LogMenu : MenuPanelWithStroke
{
	[SerializeField] private TMP_Text _logInputField;
	[SerializeField] private Button _clearLogButton;

	public UnityAction OnClearLogEvent;

	public TMP_Text LogInputField => _logInputField;

	private void OnEnable()
	{
		_clearLogButton.onClick.AddListener(ClearLog);
	}

	private void OnDisable()
	{
		_clearLogButton.onClick.RemoveListener(ClearLog);
	}

	private void ClearLog()
	{
		OnClearLogEvent?.Invoke();
	}
}