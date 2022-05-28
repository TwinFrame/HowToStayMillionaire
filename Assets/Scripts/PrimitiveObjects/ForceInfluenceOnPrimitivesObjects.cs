using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ForceInfluenceOnPrimitivesObjects : MonoBehaviour
{
	[SerializeField] private Button _restartButton;
	[SerializeField] private Toggle _forceDirection;
	[SerializeField] private TMP_InputField _forceValue;
	[SerializeField] private Button _forceButton;
	[Space]
	[Range(0, 300)]
	[SerializeField] private float defaultForce = 100f;

	public UnityAction<float, bool> OnForceEvents;
	public UnityAction OnRestartEvents;
	public UnityAction BlockHotkeyEvent;
	public UnityAction UnblockHotkeyEvent;

	private void Awake()
	{
		_forceValue.text = defaultForce.ToString();
	}

	private void OnEnable()
	{
		_forceButton.onClick.AddListener(OnClickForceButton);
		_restartButton.onClick.AddListener(OnClickRestartButton);

		_forceValue.onSelect.AddListener(OnBlockHotkey);
		_forceValue.onDeselect.AddListener(OnUnblockHotkey);
	}

	private void OnDisable()
	{
		_forceButton.onClick.RemoveListener(OnClickForceButton);
		_restartButton.onClick.RemoveListener(OnClickRestartButton);

		_forceValue.onSelect.RemoveListener(OnBlockHotkey);
		_forceValue.onDeselect.RemoveListener(OnUnblockHotkey);
	}

	private void OnClickForceButton()
	{
		if (float.TryParse(_forceValue.text, out float value))
		{
			OnForceEvents?.Invoke(value, _forceDirection.isOn);
		}
	}

	private void OnClickRestartButton()
	{
		OnRestartEvents?.Invoke();
	}

	public void OnBlockHotkey(string text)
	{
		BlockHotkeyEvent?.Invoke();
	}

	public void OnUnblockHotkey(string text)
	{
		UnblockHotkeyEvent?.Invoke();
	}
}