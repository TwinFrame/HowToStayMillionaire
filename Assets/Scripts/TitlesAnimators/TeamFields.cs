using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class TeamFields : MonoBehaviour
{
	[SerializeField] private TMP_Text _name;
	[SerializeField] private TMP_Text _bank;

	public TMP_Text Name => _name;
	public TMP_Text Bank => _bank;

	public void SetName(string name)
	{
		_name.text = name;
	}

	public void SetBank(string bank)
	{
		_bank.text = bank;
	}
}