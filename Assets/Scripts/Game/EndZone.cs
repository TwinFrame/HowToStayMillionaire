using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
	private void OnTriggerExit(Collider collider)
	{
		if (collider.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
			rigidbody.velocity *= -1;	
	}
}
