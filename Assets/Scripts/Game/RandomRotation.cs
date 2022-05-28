using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
	[SerializeField] private float _stepRotation;

	private Vector3 _currentDirection;
	private Vector3 _AddingToDirection = new Vector3();

	private void Awake()
	{
		Random.InitState(Random.Range(0, 1001));
	}

	private void LateUpdate()
	{
		_currentDirection = transform.rotation.eulerAngles.normalized;

		_AddingToDirection.y = Random.Range(0, _stepRotation);
		_AddingToDirection.z = Random.Range(0, _stepRotation);

		_currentDirection += _AddingToDirection;

		transform.Rotate(_currentDirection);
	}
}