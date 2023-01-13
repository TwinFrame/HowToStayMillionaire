using System.Collections.Generic;
using UnityEngine;

public class GameSpace : MonoBehaviour
{
	[SerializeField] private SphereCollider _endZone;

	[SerializeField] private BoxCollider _saveZone;
	[Space]
	[SerializeField] private LayerMask _layerMask;

	[Header("Service")]
	[SerializeField] private GameProperties _properties;

	private float _additionalDistance = 15f;
	private float _rayDistance = 25f;
	private Ray _ray = new Ray();
	private bool _needToFindPoint;
	private bool _isHitWithSafeArea;
	private Vector3 _hitPoint = new Vector3();
	private List<Vector3> _hitPoints = new List<Vector3>();

	private void Awake()
	{
		_endZone.radius = _properties.EndZoneRadius;
	}

	public List<Vector3> GetPointsOnEndZone(int count)
	{
		_hitPoints.Clear();

		for (int i = 0; i < count; i++)
		{
			_hitPoints.Add(GetPointOnEndZone());
		}

		return _hitPoints;
	}

	private Vector3 GetPointOnEndZone()
	{
		_needToFindPoint = true;

		while (_needToFindPoint)
		{
			_isHitWithSafeArea = false;

			_ray.origin = Random.onUnitSphere * (_endZone.radius + _additionalDistance);

			_ray.direction = Vector3.zero - _ray.origin;

			RaycastHit[] raycastHits = Physics.RaycastAll(_ray, _rayDistance, _layerMask);

			foreach (var hit in raycastHits)
			{
				if (hit.collider.gameObject.layer == 8)
				{
					Debug.Log("Попали на точку с SaveZone: " + hit.point);

					_isHitWithSafeArea = true;
				}
				else if (hit.collider.gameObject.layer == 9)
				{
					_hitPoint = hit.point;
				}
			}

			if (_isHitWithSafeArea)
			{
				_needToFindPoint = true;
			}
			else
			{
				if (_hitPoint == Vector3.zero)
					_needToFindPoint = true;
				else
					_needToFindPoint = false;
			}
		}

		return _hitPoint;
	}
}
