using UnityEngine;

[RequireComponent(typeof(Server))]

public class GameServer : MonoBehaviour
{
	[SerializeField] private Server _server;

	public Server Server => _server;
}
