using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConvertToMoneyFormat : MonoBehaviour
{
	[SerializeField] private int _numberCharsInPool;

	private string _currentReverseBank;
	private string _currentBank;
	private int _currentChar;

	public string MoneyFormat(string money, char monetaryUnit)
	{
		_currentReverseBank = string.Empty;
		_currentBank = string.Empty;
		_currentChar = 1;

		for (int i = money.Length - 1; i >= 0; i--)
		{
			_currentReverseBank += money[i];

			if (_currentChar % _numberCharsInPool == 0)
				_currentReverseBank += " ";

			_currentChar++;
		}

		for (int i = _currentReverseBank.Length - 1; i >= 0; i--)
		{

			_currentBank += _currentReverseBank[i];
		}

		_currentBank += " " + monetaryUnit;

		return _currentBank;
	}
}
