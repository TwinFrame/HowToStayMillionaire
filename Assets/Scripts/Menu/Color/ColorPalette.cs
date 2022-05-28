using UnityEngine;

public class ColorPalette : MonoBehaviour
{
	[SerializeField] private string _name;
	[Space]
	[SerializeField] private bool _isUserAllowedChange;
	[Space]
	[SerializeField] private Color _mainColor;
	[SerializeField] private Color _slaveColor;
	[SerializeField] private Color _add1Color;
	[SerializeField] private Color _add2Color;
	[Space]
	[SerializeField] private Color _textColor;
	[SerializeField] private Color _selectedColor;
	[SerializeField] private Color _rightColor;
	[SerializeField] private Color _wrongColor;

	public string Name => _name;
	public bool IsUserAllowedChange => _isUserAllowedChange;
	public Color MainColor => _mainColor;
	public Color SlaveColor => _slaveColor;
	public Color Add1Color => _add1Color;
	public Color Add2Color => _add2Color;
	public Color TextColor => _textColor;
	public Color SelectedColor => _selectedColor;
	public Color RightColor => _rightColor;
	public Color WrongColor => _wrongColor;

	public void Set(string name, Color mainColor, Color slaveColor, Color add1Color, Color add2Color, Color textColor,
		Color selectedColor, Color rightColor, Color wrongColor, bool isUserAllowedChange)
	{
		_name = name;
		_isUserAllowedChange = isUserAllowedChange;

		SetColors(mainColor, slaveColor, add1Color, add2Color, textColor, selectedColor, rightColor, wrongColor);
	}

	public void SetColors(Color mainColor, Color slaveColor, Color add1Color, Color add2Color,
		Color textColor, Color selectedColor, Color rightColor, Color wrongColor)
	{
		_mainColor = mainColor;
		_slaveColor = slaveColor;
		_add1Color = add1Color;
		_add2Color = add2Color;
		_textColor = textColor;
		_selectedColor = selectedColor;
		_rightColor = rightColor;
		_wrongColor = wrongColor;
	}

	public void ChangeColor(TypesOfGameColor typeOfColor, Color color)
	{
		switch (typeOfColor)
		{
			case TypesOfGameColor.Main:
				_mainColor = color;
				break;
			case TypesOfGameColor.Slave:
				_slaveColor = color;
				break;
			case TypesOfGameColor.Additional1:
				_add1Color = color;
				break;
			case TypesOfGameColor.Additional2:
				_add2Color = color;
				break;
			case TypesOfGameColor.Text:
				_textColor = color;
				break;
			case TypesOfGameColor.Selected:
				_selectedColor = color;
				break;
			case TypesOfGameColor.Right:
				_rightColor = color;
				break;
			case TypesOfGameColor.Wrong:
				_wrongColor = color;
				break;
			default:
				Debug.Log($"Not find GameColor {typeOfColor}");
				break;
		}
	}
}
