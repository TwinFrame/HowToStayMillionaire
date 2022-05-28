using UnityEngine;

[RequireComponent(typeof(Client))]

public class GameClient : MonoBehaviour
{
	[SerializeField] private Client _client;

	public Client Client => _client;
}
