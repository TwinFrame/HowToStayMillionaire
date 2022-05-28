using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FieldOfViewCounter : MonoBehaviour
{
	[SerializeField] private Camera _camera;
	[SerializeField] TMP_Text text;
	[SerializeField] private float _refreshEverySecond;

	private float _fov;
	private WaitForSeconds _waitForSeconds;

	private InputGame _input;
	private int _value;

	private void OnEnable()
	{
		_input = new InputGame();
		_input.Enable();
		_input.Game.PlusFieldOfView.performed += ctx => OnPlusFieldOfView();
		_input.Game.MinusFieldOfView.performed += ctx => OnMinusFieldOfView();
	}

	private void OnDisable()
	{
		_input.Game.PlusFieldOfView.performed -= ctx => OnPlusFieldOfView();
		_input.Game.MinusFieldOfView.performed -= ctx => OnMinusFieldOfView();
	}

	private void Awake()
	{
		_waitForSeconds = new WaitForSeconds(_refreshEverySecond);
	}

	private IEnumerator Start()
	{
		while (true)
		{
			_fov = _camera.fieldOfView;

			text.text = _fov.ToString();

			yield return _waitForSeconds;
		}
	}

	private void OnPlusFieldOfView()
	{
		_camera.fieldOfView += 1;
	}

	private void OnMinusFieldOfView()
	{
		_camera.fieldOfView -= 1;
	}
}
