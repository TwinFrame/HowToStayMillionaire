using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuDisplayWindow : MenuWindow
{
	[SerializeField] private TMP_Dropdown _resolutionDropdown;
	[SerializeField] private Button _refreshButton;
	[SerializeField] private Button _quitButton;
	[SerializeField] private Button _customResolutionButton;
	[SerializeField] private TMP_Text _displayText;
	[SerializeField] private Toggle _fullscreenToggle;
	[SerializeField] private MenuResolutionWindow _resolutionWindow;

	public UnityAction<int, int> OnChangedResolutionEvent;
	public UnityAction<bool> OnSwitchedFullscreenEvent;
	public UnityAction OnRefreshDisplayInfoEvent;
	public UnityAction OnCloseWindowEvent;

	protected virtual void OnEnable()
	{
		_resolutionDropdown.onValueChanged.AddListener(OnChangeResolution);
		_refreshButton.onClick.AddListener(OnRefreshDisplayInfo);
		_quitButton.onClick.AddListener(OnCloseWindow);
		_customResolutionButton.onClick.AddListener(OnOpenResolutionWindow);
		_fullscreenToggle.onValueChanged.AddListener(OnSwitchedFullscreen);

		_resolutionWindow.OnSetUserResolutionEvent += (width, height) => OnSetResolution(width, height);
		_resolutionWindow.OnQuitWindowEvent += CloseResolutionWindow;
	}

	protected virtual void OnDisable()
	{
		_resolutionDropdown.onValueChanged.RemoveListener(OnChangeResolution);
		_refreshButton.onClick.RemoveListener(OnRefreshDisplayInfo);
		_quitButton.onClick.RemoveListener(OnCloseWindow);
		_customResolutionButton.onClick.RemoveListener(OnOpenResolutionWindow);
		_fullscreenToggle.onValueChanged.RemoveListener(OnSwitchedFullscreen);

		_resolutionWindow.OnSetUserResolutionEvent -= (width, height) => OnSetResolution(width, height);
		_resolutionWindow.OnQuitWindowEvent -= CloseResolutionWindow;
	}

	public void SetDisplayInfo(string displayInfo, bool isFullscreen)
	{
		_displayText.text = displayInfo;
		_fullscreenToggle.SetIsOnWithoutNotify(isFullscreen);
	}

	public void OnRefreshDisplayInfo()
	{
		OnRefreshDisplayInfoEvent?.Invoke();
	}

	public void OnCloseWindow()
	{
		OnCloseWindowEvent?.Invoke();
	}

	protected void OnChangeResolution(int resolution)
	{
		switch (resolution)
		{
			default:
			case 0:
				OnChangedResolutionEvent?.Invoke(640, 360);
				break;
			case 1:
				OnChangedResolutionEvent?.Invoke(854, 480);
				break;
			case 2:
				OnChangedResolutionEvent?.Invoke(960, 540);
				break;
			case 3:
				OnChangedResolutionEvent?.Invoke(1024, 576);
				break;
			case 4:
				OnChangedResolutionEvent?.Invoke(1280, 720);
				break;
			case 5:
				OnChangedResolutionEvent?.Invoke(1600, 900);
				break;
			case 6:
				OnChangedResolutionEvent?.Invoke(1920, 1080);
				break;
			case 7:
				OnChangedResolutionEvent?.Invoke(2048, 1152);
				break;
			case 8:
				OnChangedResolutionEvent?.Invoke(2560, 1440);
				break;
			case 9:
				OnChangedResolutionEvent?.Invoke(2880, 1620);
				break;
			case 10:
				OnChangedResolutionEvent?.Invoke(3200, 1800);
				break;
			case 11:
				OnOpenResolutionWindow();
				break;
		}
	}

	private void OnSetResolution(int width, int height)
	{
		OnChangedResolutionEvent?.Invoke(width, height);

		CloseResolutionWindow();
	}

	private void OnSwitchedFullscreen(bool isFullscreen)
	{
		OnSwitchedFullscreenEvent?.Invoke(isFullscreen);
	}

	private void OnOpenResolutionWindow()
	{
		_resolutionWindow.gameObject.SetActive(true);
		_resolutionWindow.DisableSetButton();
	}

	private void CloseResolutionWindow()
	{
		_resolutionWindow.gameObject.SetActive(false);

		//_resolutionDropdown.SetValueWithoutNotify(_resolutionDropdown.options.Count - 1);
	}
}
