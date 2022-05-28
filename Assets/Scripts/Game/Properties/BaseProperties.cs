using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProperties : MonoBehaviour
{
	[Header("Service")]
	[SerializeField] private MenuThemeChanger _menuThemeChanger;
	[SerializeField] private DisplayController _displayController;
	[SerializeField] private TextureConverter _textureConverter;

	public MenuThemeChanger ThemeMenuChanger => _menuThemeChanger;
	public DisplayController DisplayController => _displayController;

	public TextureConverter TextureConverter =>  _textureConverter;

}