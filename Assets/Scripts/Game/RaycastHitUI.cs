using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RaycastHitUI : MonoBehaviour
{
	[SerializeField] EventSystem _eventSystem;

	private GraphicRaycaster _raycaster;
	private PointerEventData _pointerEventData;

	private void Start()
	{
		_raycaster = GetComponent<GraphicRaycaster>();
	}

	private void Update()
	{
		/*
		if (Input.GetMouseButtonDown(0))
		{
			_pointerEventData = new PointerEventData(_eventSystem);

			_pointerEventData.position = Input.mousePosition;

			List<RaycastResult> results = new List<RaycastResult>();

			_raycaster.Raycast(_pointerEventData, results);

			foreach (RaycastResult result in results)
				Debug.Log("Hit " + result.gameObject.name);
		}
		*/
	}
}
