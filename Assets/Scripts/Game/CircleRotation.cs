using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotation : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _direction = new Vector3(1, 1, 0);

    void Update()
    {
        transform.Rotate(_direction, _speed * Time.deltaTime);
    }
}
