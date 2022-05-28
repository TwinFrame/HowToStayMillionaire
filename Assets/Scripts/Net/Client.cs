using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Client : MonoBehaviour
{
	public static Client Instance { set; get; }

	private void Awake()
	{
		Instance = this;
	}

	public NetworkDriver Driver;
	private NetworkConnection _connection;

	private bool _isActive = false;

	public Action ConnectionDroppedEvent;
	public Action ActivateEvent;
	public Action ConnectedEvent;
	public Action ShutdownEvent;

	public bool IsActive => _isActive;
	public NetworkConnection Connection => _connection;

	//Methods
	public void Init(string ipAddress, ushort port)
	{
		Driver = NetworkDriver.Create();
		NetworkEndPoint endPoint = NetworkEndPoint.Parse(ipAddress, port);

		_connection = Driver.Connect(endPoint);

		Debug.Log("Attemping to connect to Server on " + endPoint.Address);

		_isActive = true;

		ActivateEvent?.Invoke();

		RegisterToEvent();
	}

	public void Shutdown()
	{
		if (_isActive)
		{
			ShutdownEvent?.Invoke();

			UnregisterToEvent();

			Driver.Dispose();
			_connection = default(NetworkConnection);
			_isActive = false;
		}
	}

	public void OnDestroy() //maybe private
	{
		Shutdown();
	}


	public void Update()
	{
		if (!_isActive)
			return;

		Driver.ScheduleUpdate().Complete();

		CheckAlive();

		UpdateMessagePump();
	}

	private void CheckAlive()
	{
		if (!_connection.IsCreated && _isActive)
		{
			ConnectionDroppedEvent?.Invoke();
			Shutdown();

			Debug.Log("Something went wrong, lost connection to Server");
		}
	}

	private void UpdateMessagePump()
	{
		DataStreamReader stream;

		NetworkEvent.Type cmd;

		while ((cmd = _connection.PopEvent(Driver, out stream)) != NetworkEvent.Type.Empty)
		{
			if (cmd == NetworkEvent.Type.Connect)
			{
				SendToServer(new NetWelcome());

				ConnectedEvent?.Invoke();
				Debug.Log("We`re connected");
			}
			else if (cmd == NetworkEvent.Type.Data)
			{
				NetUtility.OnData(stream, default(NetworkConnection));
			}
			else if (cmd == NetworkEvent.Type.Disconnect)
			{
				_connection = default(NetworkConnection);
				ConnectionDroppedEvent?.Invoke();
				Shutdown();

				Debug.Log("Client got disconnected from server ");
			}
		}

	}

	//Server specific
	public void SendToServer(NetMessage msg)
	{
		DataStreamWriter writer;
		Driver.BeginSend(_connection, out writer);
		msg.Serialize(ref writer);
		Driver.EndSend(writer);
	}

	// Event parsing
	private void RegisterToEvent()
	{
		NetUtility.C_KEEP_ALIVE += OnKeepAlive;
	}
	private void UnregisterToEvent()
	{
		NetUtility.C_KEEP_ALIVE -= OnKeepAlive;
	}

	private void OnKeepAlive(NetMessage msg)
	{
		// Send it back, to keep both side alive
		SendToServer(msg);
	}
}
