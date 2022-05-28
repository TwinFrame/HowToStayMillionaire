using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
	[SerializeField] private string _name;
	
	private int _bank = 0;
	private bool _isAsked;
	private bool _isReady;
	private bool _isConfirmNameWhenPreparing;
	private bool _isWinner;

	public string Name => _name;
	public int Bank => _bank;
	public bool IsAsked => _isAsked;
	public bool IsReady => _isReady;
	public bool IsWinner => _isWinner;
	public bool IsConfirmNameWhenPreparing => _isConfirmNameWhenPreparing;

	public void RemovingMoney(int money)
	{
		if (_bank - money <= 0)
		{
			_bank = 0;

			Debug.Log($"Команда {_name} банкрот.");

			return;
		}

		_bank -= money;		
	}

	public void AddingMoney(int money)
	{
		_bank += money;
	}

	public void ChangeTeamName(string newName)
	{
		_name = newName;
	}

	public void ResetTeam()
	{
		_bank = 0;
		ResetIsAsked();
	}

	public void ResetIsAsked()
	{
		_isAsked = false;
		_isReady = false;
		_isWinner = false;
		_isConfirmNameWhenPreparing = false;
	}

	public void OnIsAsked()
	{
		_isAsked = true;
	}

	public void OnIsReady()
	{
		_isReady = true;
	}

	public void OnIsWinner()
	{
		_isWinner = true;
	}

	public void ConfirmNameWhenPreparing()
	{
		_isConfirmNameWhenPreparing = true;
	}
}
