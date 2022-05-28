using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Server : MonoBehaviour
{
	public static Server Instance { set; get; }

	private void Awake()
	{
		Instance = this;
	}

	public NetworkDriver Driver;
	private NativeList<NetworkConnection> _connections;

	private bool _isActive = false;
	private const float _keepAliveTickRate = 20;
	private float _lastKeepAlive;

	public bool IsActive => _isActive;

	public Action ConnectionDroppedEvent;
	public Action NoClientConnectionEvent;
	public Action HaveGotClientConnectionEvent;
	public Action ShutdownEvent;

	//Methods
	public void Init(ushort port)
	{
		Driver = NetworkDriver.Create();
		NetworkEndPoint endPoint = NetworkEndPoint.AnyIpv4;
		endPoint.Port = port;

		if (Driver.Bind(endPoint) != 0)
		{
			Debug.Log("Unable to bind on port " + endPoint.Port);
			return;
		}
		else
		{
			Driver.Listen();

			if (!CheckingClientConnections())
				NoClientConnectionEvent?.Invoke();

			Debug.Log("Currently listening on port " + endPoint.Port);
		}

		_connections = new NativeList<NetworkConnection>(4, Allocator.Persistent);

		_isActive = true;
	}

	public void Shutdown()
	{
		if (_isActive)
		{
			ShutdownEvent?.Invoke();

			Driver.Dispose();
			_connections.Dispose();
			_isActive = false;
		}
		else
			ShutdownEvent?.Invoke();
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

		KeepAlive();

		CleanupConnections();
		AcceptNewConnections();
		UpdateMessagePump();
	}

	private void KeepAlive()
	{
		if (Time.time - _lastKeepAlive > _keepAliveTickRate)
		{
			_lastKeepAlive = Time.time;
			Broadcast(new NetKeepAlive());
		}
	}

	private void CleanupConnections()
	{
		for (int i = 0; i < _connections.Length; i++)
		{
			if (!_connections[i].IsCreated)
			{
				_connections.RemoveAtSwapBack(i);
				--i;

				if (!CheckingClientConnections())
					NoClientConnectionEvent?.Invoke();
			}
		}
	}

	private void AcceptNewConnections()
	{
		NetworkConnection networkConnection;

		while ((networkConnection = Driver.Accept()) != default(NetworkConnection))
		{
			_connections.Add(networkConnection);

			if (CheckingClientConnections())
				HaveGotClientConnectionEvent?.Invoke();
		}
	}

	private void UpdateMessagePump()
	{
		DataStreamReader stream;

		for (int i = 0; i < _connections.Length; i++)
		{
			NetworkEvent.Type cmd;

			while ((cmd = Driver.PopEventForConnection(_connections[i], out stream)) != NetworkEvent.Type.Empty)
			{
				if (cmd == NetworkEvent.Type.Data)
				{
					NetUtility.OnData(stream, _connections[i], this);
				}
				else if (cmd == NetworkEvent.Type.Disconnect)
				{
					_connections[i] = default(NetworkConnection);
					ConnectionDroppedEvent?.Invoke();

					//Shutdown(); //если игра была только для двух игроков, то актуально выключать сервер, при отключении второго.

					Debug.Log("Client disconnected from server ");
				}
			}
		}
	}

	//Server specific
	public void SendToClient(NetworkConnection networkConnection, NetMessage msg)
	{
		DataStreamWriter writer;
		Driver.BeginSend(networkConnection, out writer);
		msg.Serialize(ref writer);
		Driver.EndSend(writer);
	}

	public void Broadcast(NetMessage msg)
	{
		for (int i = 0; i < _connections.Length; i++)
		{
			if (_connections[i].IsCreated)
			{
				SendToClient(_connections[i], msg);
				Debug.Log($"Sending {msg.Code} to: {_connections[i].InternalId}");
			}
		}
	}

	public bool CheckingClientConnections()
	{
		if (_connections.IsEmpty || _connections.Length <= 0)
			return false;
		else
			return true;
	}
}