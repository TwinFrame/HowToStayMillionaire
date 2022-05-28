using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class TabButton : MonoBehaviour
{
	[SerializeField] protected TypesOfTab _typesMenu;

	public TypesOfTab TypesMenuReadOnly => _typesMenu;

	public void SelectButton(Sprite sprite)
	{
		if (TryGetComponent<Button>(out Button button))
		{
			button.image.sprite = sprite;
		}
	}
}
