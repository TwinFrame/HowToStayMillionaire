using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameColorChanger : MonoBehaviour
{
	[SerializeField] private List<ColorPalette> _palettes;
	[Space]
	[Range(0, 1)]
	[SerializeField] private float _weightLigtherBG;
	[Header("Logo")]
	[SerializeField] private Texture2D _transparentTexture;
	[SerializeField] private Material _logoMaterial;
	[Space]
	[SerializeField] private Material _material1;
	[SerializeField] private Material _material2;
	[Space]
	[SerializeField] private Material _material3;
	[SerializeField] private Material _material4;
	[Space]
	[SerializeField] private Material _material5;
	[SerializeField] private Material _material6;
	[Space]
	[SerializeField] private Material _mainBarMaterial;
	[SerializeField] private Material _glassIndicator;
	[Space]
	[SerializeField] private SpriteShapeRenderer[] _audioShapes;
	[SerializeField] private SpriteShapeRenderer[] _audioMaskShapes;
	[Space]
	[SerializeField] protected Material[] _gameFontMaterials;
	[Space]
	[SerializeField] private Camera _mainCamera;
	[SerializeField] private Camera _previewCamera;

	private Texture2D _logo;
	private bool _isLogo;
	private string _logoIsNotLoaded = "The logo not loaded.";

	private int _currentNumPalette;

	public int CurrentNumPalette => _currentNumPalette;

	public List<ColorPalette> Palettes => _palettes;

	public UnityAction<Texture2D, string> OnChangedLogoEvent;
	public UnityAction<List<ColorPalette>, int> OnSetColorsFromPalette;

	private void Awake()
	{
		if (TryGetLogoOnAwake(out Texture2D logo))
		{
			_logo = logo;
			_isLogo = true;
		}
		else
		{
			_logo = _transparentTexture;
			_isLogo = false;

			PlayerPrefs.DeleteKey("LogoPath");
			PlayerPrefs.Save();
		}

		if (PlayerPrefs.HasKey("CurrentNumPalette"))
		{
			if (PlayerPrefs.GetInt("CurrentNumPalette") < _palettes.Count)
				SaveCurrentPalette(PlayerPrefs.GetInt("CurrentNumPalette"));
			else
				SaveCurrentPalette(0);
		}
		else
		{
			SaveCurrentPalette(0);
		}

		CheckingUserSavedPalettes();
	}

	private void Start()
	{
		SetLogoOnPrimitiveObjects();

		SetColorsFromCurrentPalette();
	}

	#region Colors

	public void ChangePalette(int numPalette)
	{
		SaveCurrentPalette(numPalette);

		SetColorsFromCurrentPalette();
	}

	public void ChangeColorInPalette(int numPalette, TypesOfGameColor typeOfColor, Color color)
	{
		if (!_palettes[numPalette].IsUserAllowedChange)
			return;

		_palettes[numPalette].ChangeColor(typeOfColor, color);

		SaveColorInUserPalette(_palettes[numPalette], typeOfColor, color);

		SetColorsFromCurrentPalette();
	}

	public List<ColorPalette> GetPalettes(out int currentNumPalette)
	{
		currentNumPalette = _currentNumPalette;
		//paletteNames = GetPaletteNames();
		return _palettes;
	}

	public Color GetTextColor()
	{
		return _palettes[_currentNumPalette].TextColor;
	}

	public Color GetSelectedColor()
	{
		return _palettes[_currentNumPalette].SelectedColor;
	}

	public Color GetRightColor()
	{
		return _palettes[_currentNumPalette].RightColor;
	}

	public Color GetWrongColor()
	{
		return _palettes[_currentNumPalette].WrongColor;
	}

	private void SetColorsFromCurrentPalette()
	{
		ChangeMainColor(_palettes[_currentNumPalette].MainColor);
		ChangeSlaveColor(_palettes[_currentNumPalette].SlaveColor);
		ChangeAdd1Color(_palettes[_currentNumPalette].Add1Color);
		ChangeAdd2Color(_palettes[_currentNumPalette].Add2Color);
		ChangeTextColor(_palettes[_currentNumPalette].TextColor);

		OnSetColorsFromPalette?.Invoke(_palettes, _currentNumPalette);

	}

	#endregion

	#region Logo

	public bool TryGetLogo(out string path)
	{
		if (_isLogo)
		{
			path = PlayerPrefs.GetString("LogoPath");
			return true;
		}
		else
		{
			path = _logoIsNotLoaded;
			return false;
		}
	}

	public void SetLogoOnPrimitiveObjects()
	{
		_logoMaterial.SetTexture("_Logo", _logo);

		if (_isLogo)
			OnChangedLogoEvent?.Invoke(_logo, PlayerPrefs.GetString("LogoPath"));
		else
			OnChangedLogoEvent?.Invoke(_logo, _logoIsNotLoaded);
	}

	public void DeleteLogo()
	{
		PlayerPrefs.DeleteKey("LogoPath");
		PlayerPrefs.Save();

		_logo = _transparentTexture;
		_isLogo = false;

		SetLogoOnPrimitiveObjects();
	}

	public void UserLoadedLogo(Texture2D logo, string path)
	{
		PlayerPrefs.SetString("LogoPath", path);
		PlayerPrefs.Save();

		_logo = logo;
		_isLogo = true;

		SetLogoOnPrimitiveObjects();
	}

	private bool TryGetLogoOnAwake(out Texture2D logo)
	{
		Texture2D texture2D = new Texture2D(1, 1);

		if (PlayerPrefs.HasKey("LogoPath"))
		{
			if (File.Exists(PlayerPrefs.GetString("LogoPath")))
			{
				byte[] bytes = File.ReadAllBytes(PlayerPrefs.GetString("LogoPath"));

				if (ImageConversion.LoadImage(texture2D, bytes, false))
				{
					logo = texture2D;
					return true;
				}
				else
				{
					logo = texture2D;
					return false;
				}
			}
			else
			{
				Debug.Log("Logo is not exists.");

				logo = texture2D;
				return false;
			}
		}
		else
		{
			Debug.Log("The key LogoPath does not exist");

			logo = texture2D;
			return false;
		}
	}
	#endregion

	private void ChangeMainColor(Color color)
	{
		_material1.SetColor("_MainColor", color);
		_material2.SetColor("_SecondColor", color);

		_mainBarMaterial.SetColor("_MainColor", color);

		color = color / (1 - _weightLigtherBG);
		_mainCamera.backgroundColor = color;
		_previewCamera.backgroundColor = color;
	}

	private void ChangeAdd1Color(Color color)
	{
		_material1.SetColor("_SecondColor", color);
		_material2.SetColor("_MainColor", color);
	}

	private void ChangeSlaveColor(Color color)
	{
		_material3.SetColor("_MainColor", color);
		_material4.SetColor("_SecondColor", color);

		ChangeAudioMaskShapeColor(color);
	}

	private void ChangeAdd2Color(Color color)
	{
		_material3.SetColor("_SecondColor", color);
		_material4.SetColor("_MainColor", color);

		_logoMaterial.SetColor("_MainColor", color);

		ChangeAudioShapeColor(color);
	}

	private void ChangeTextColor(Color color)
	{
		SetFontColor(color);

		_glassIndicator.SetColor("_MainColor", color);
	}

	private void SetFontColor(Color color)
	{
		foreach (var material in _gameFontMaterials)
			material.SetColor("_FaceColor", color);
	}

	private void ChangeAudioShapeColor(Color color)
	{
		foreach (var shape in _audioShapes)
			shape.color = color;
	}

	private void ChangeAudioMaskShapeColor(Color color)
	{
		foreach (var shape in _audioMaskShapes)
			shape.color = color;
	}

	private void SaveCurrentPalette(int numPalette)
	{
		_currentNumPalette = numPalette;

		PlayerPrefs.SetInt("CurrentNumPalette", _currentNumPalette);
		PlayerPrefs.Save();
	}

	private void SaveColorInUserPalette(ColorPalette colorPalette, TypesOfGameColor gameColor, Color color)
	{
		PlayerPrefs.SetFloat(colorPalette.Name + gameColor + "R", color.r);
		PlayerPrefs.SetFloat(colorPalette.Name + gameColor + "G", color.g);
		PlayerPrefs.SetFloat(colorPalette.Name + gameColor + "B", color.b);
		PlayerPrefs.SetFloat(colorPalette.Name + gameColor + "A", color.a);
	}

	private void CheckingUserSavedPalettes()
	{
		List<ColorPalette> userColorPalettes = new List<ColorPalette>();

		foreach (var palette in _palettes)
		{
			if (palette.IsUserAllowedChange)
				userColorPalettes.Add(palette);
		}

		if (userColorPalettes.Count > 0)
		{
			foreach (var colorPalette in userColorPalettes)
			{
				if (PlayerPrefs.HasKey(colorPalette.Name + TypesOfGameColor.Main + "R") &&
					PlayerPrefs.HasKey(colorPalette.Name + TypesOfGameColor.Slave + "R") &&
					PlayerPrefs.HasKey(colorPalette.Name + TypesOfGameColor.Additional1 + "R") &&
					PlayerPrefs.HasKey(colorPalette.Name + TypesOfGameColor.Additional2 + "R") &&
					PlayerPrefs.HasKey(colorPalette.Name + TypesOfGameColor.Text + "R") &&
					PlayerPrefs.HasKey(colorPalette.Name + TypesOfGameColor.Selected + "R") &&
					PlayerPrefs.HasKey(colorPalette.Name + TypesOfGameColor.Right + "R") &&
					PlayerPrefs.HasKey(colorPalette.Name + TypesOfGameColor.Wrong + "R"))
				{
					Color mainColor = new Color(PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Main + "R"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Main + "G"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Main + "B"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Main + "A"));

					Color slaveColor = new Color(PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Slave + "R"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Slave + "G"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Slave + "B"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Slave + "A"));

					Color add1Color = new Color(PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Additional1 + "R"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Additional1 + "G"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Additional1 + "B"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Additional1 + "A"));

					Color add2Color = new Color(PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Additional2 + "R"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Additional2 + "G"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Additional2 + "B"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Additional2 + "A"));

					Color textColor = new Color(PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Text + "R"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Text + "G"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Text + "B"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Text + "A"));

					Color selectedColor = new Color(PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Selected + "R"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Selected + "G"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Selected + "B"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Selected + "A"));

					Color rightColor = new Color(PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Right + "R"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Right + "G"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Right + "B"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Right + "A"));

					Color wrongColor = new Color(PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Wrong + "R"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Wrong + "G"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Wrong + "B"),
						PlayerPrefs.GetFloat(colorPalette.Name + TypesOfGameColor.Wrong + "A"));

					colorPalette.SetColors(mainColor, slaveColor, add1Color, add2Color, textColor, selectedColor, rightColor, wrongColor);
				}
				else
				{
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Main + "R", colorPalette.MainColor.r);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Main + "G", colorPalette.MainColor.g);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Main + "B", colorPalette.MainColor.b);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Main + "A", colorPalette.MainColor.a);

					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Slave + "R", colorPalette.SlaveColor.r);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Slave + "G", colorPalette.SlaveColor.g);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Slave + "B", colorPalette.SlaveColor.b);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Slave + "A", colorPalette.SlaveColor.a);

					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Additional1 + "R", colorPalette.Add1Color.r);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Additional1 + "G", colorPalette.Add1Color.g);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Additional1 + "B", colorPalette.Add1Color.b);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Additional1 + "A", colorPalette.Add1Color.a);

					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Additional2 + "R", colorPalette.Add2Color.r);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Additional2 + "G", colorPalette.Add2Color.g);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Additional2 + "B", colorPalette.Add2Color.b);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Additional2 + "A", colorPalette.Add2Color.a);

					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Text + "R", colorPalette.TextColor.r);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Text + "G", colorPalette.TextColor.g);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Text + "B", colorPalette.TextColor.b);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Text + "A", colorPalette.TextColor.a);

					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Selected + "R", colorPalette.SelectedColor.r);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Selected + "G", colorPalette.SelectedColor.g);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Selected + "B", colorPalette.SelectedColor.b);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Selected + "A", colorPalette.SelectedColor.a);

					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Right + "R", colorPalette.RightColor.r);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Right + "G", colorPalette.RightColor.g);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Right + "B", colorPalette.RightColor.b);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Right + "A", colorPalette.RightColor.a);

					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Wrong + "R", colorPalette.WrongColor.r);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Wrong + "G", colorPalette.WrongColor.g);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Wrong + "B", colorPalette.WrongColor.b);
					PlayerPrefs.SetFloat(colorPalette.Name + TypesOfGameColor.Wrong + "A", colorPalette.WrongColor.a);
				}
			}
		}
	}
}
