using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
	[SerializeField] TMP_Text text;
	[SerializeField] private float _refreshEverySecond;
	private int _fps;
	private WaitForSeconds _waitForSeconds;

	private void Awake()
	{
		_waitForSeconds = new WaitForSeconds(_refreshEverySecond);
	}

	private IEnumerator Start()
	{
		while (true)
		{
			_fps = (int)(1f / Time.unscaledDeltaTime);

			text.text = _fps.ToString();

			yield return _waitForSeconds;
		}
	}
}
