using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
//using SimpleFileBrowser;

public class OpenMenuOnMainDisplay : MonoBehaviour
{
	[SerializeField] private Camera _gameCamera;
	[SerializeField] private GameMenu _gameMenu;
	[SerializeField] private Toggle _openMenuToggle;
	//[Header("Service")]
	//[SerializeField] private Game _game;

	private Canvas _menuCanvas;
	private Camera _menuCamera;
	private Vector3 _mainCameraGamePosition;
	private Vector3 _mainCameraMenuPosition = new Vector3();
	private bool _toggleIsOn;
	private int _gameDisplay;
	private int _menuDisplay;

	private InputGame _inputs;

	private void Awake()
	{
		_menuCanvas = _gameMenu.GetComponent<Canvas>();

		_menuCamera = _menuCanvas.worldCamera;

		_mainCameraGamePosition = _gameCamera.transform.position;

		_mainCameraMenuPosition.Set(_gameMenu.transform.position.x, _gameMenu.transform.position.y, _gameCamera.transform.position.z);

		_gameDisplay = _gameCamera.targetDisplay;
		_menuDisplay = _menuCamera.targetDisplay;
	}

	private void OnEnable()
	{
		_inputs = new InputGame();
		_inputs.Enable();

		_inputs.Game.Menu.performed += ctx => OnOpenMenu();

		_openMenuToggle.onValueChanged.AddListener(OpenMenu);
	}

	private void OnDisable()
	{
		_inputs.Game.Menu.performed -= ctx => OnOpenMenu();

		_openMenuToggle.onValueChanged.RemoveListener(OpenMenu);
	}

	public void OpenMenu(bool isOn)
	{
		if (isOn)
		{
			_gameCamera.transform.position = _mainCameraMenuPosition;
			_gameCamera.GetUniversalAdditionalCameraData().renderPostProcessing = false;

			_menuCanvas.worldCamera = _gameCamera;
		}
		else
		{
			_gameCamera.transform.position = _mainCameraGamePosition;
			_gameCamera.GetUniversalAdditionalCameraData().renderPostProcessing = true;

			_menuCanvas.worldCamera = _menuCamera;
		}
	}

	private void OnOpenMenu()
	{
		if (_gameMenu.AreHotkeysBlocked)
			return;

		if (_openMenuToggle.isOn)
			_toggleIsOn = false;
		else
			_toggleIsOn = true;

		_openMenuToggle.isOn = _toggleIsOn;
	}
}
