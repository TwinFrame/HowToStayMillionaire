using System.Collections.Generic;
using UnityEngine;

public class EffectorForPrimitives : MonoBehaviour
{
	private Vector3 _currentNormalizeDirection = new Vector3();
	private float _randomForce;
	/*
	private void Update()
	{
		ShowRay();
	}
	*/
	public void AttractionToCenter(List<Rigidbody> rigidbodies, float force, bool isRandomizingForce = false)
	{
		if (isRandomizingForce)
		{
			foreach (var rigidbody in rigidbodies)
			{
				_randomForce = Random.Range(0.8f, 1.2f);

				//как разберешься. поменяй обратно transform.position на Vector3.zero
				_currentNormalizeDirection = GetNormalizeDirection(rigidbody.transform.position, Vector3.zero);

				AddForce(rigidbody, force * _randomForce, _currentNormalizeDirection);
			}
		}
		else
		{
			foreach (var rigidbody in rigidbodies)
			{
				_currentNormalizeDirection = GetNormalizeDirection(rigidbody.transform.position, transform.position);
				AddForce(rigidbody, force, _currentNormalizeDirection);
			}
		}
	}

	public void AttractionInDirection(List<Rigidbody> rigidbodies, float force, Vector3 direction)
	{
		foreach (var rigidbody in rigidbodies)
			AddForce(rigidbody, force, direction);
	}

	private void AddForce(Rigidbody rigidbody, float force, Vector3 direction)
	{
		rigidbody.AddForce(direction * force, ForceMode.Acceleration);
		rigidbody.AddRelativeTorque(direction * (Mathf.Abs(force) / 2), ForceMode.Force);
	}

	public void ResetVelocity(List<Rigidbody> rigidbodies)
	{
		foreach (var rigidbody in rigidbodies)
			rigidbody.velocity = Vector3.zero;
	}

	private Vector3 GetNormalizeDirection(Vector3 startPoint, Vector3 targetPoint)
	{
		Vector3 direction = targetPoint - startPoint;
		//Vector3 direction = startPoint - targetPoint;

		direction.Normalize();

		return direction;
	}
}
