using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using System.Collections;

public class CustomNetworkManager : NetworkManager
{
	/// <summary>
	/// The player spawn location.
	/// </summary>
	[SerializeField]
	Transform playerSpawnLocation;

	/// <summary>
	/// The players list using the MongoDB architecture.
	/// </summary>
	private Dictionary<int, PlayerInfo> _players = new Dictionary<int, PlayerInfo>();

	/// <summary>
	/// The players list using the MySQL archtiecture.
	/// </summary>
	private Dictionary<int, PlayerInfoSql> _playersSql = new Dictionary<int, PlayerInfoSql>();

	/// <summary>
	/// The player count. Used as a key identifier, the built in unity one seems
	/// like it doesnt change enough.
	/// </summary>
	public static int playerCount = 0;

	/// <summary>
	/// Provides the list to the initializer to sync the player.
	/// </summary>
	/// <value>
	/// The synchronize players.
	/// </value>
	public Dictionary<int, PlayerInfo> SyncPlayers
	{
		get
		{
			return _players;
		}
	}

	/// <summary>
	/// Gets the synchronize players SQL.
	/// </summary>
	/// <value>
	/// The synchronize players SQL.
	/// </value>
	public Dictionary<int, PlayerInfoSql> SyncPlayersSql
	{
		get
		{
			return _playersSql;
		}
	}

	/// <summary>
	/// Called when [server ready].
	/// </summary>
	/// <param name="conn">The connection.</param>
	public override void OnServerReady(NetworkConnection conn)
	{
		NetworkServer.SetClientReady(conn);
	}

	/// <summary>
	/// called when a new player is added for a client.
	/// </summary>
	/// <param name="conn">The connection.</param>
	/// <param name="playerControllerId">The player controller identifier.</param>
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Profiler.BeginSample("On server Create.");

		NetworkInitializer initializerScript = GetComponent<NetworkInitializer>();

		if (initializerScript.InitialDBType == DatabaseType.MongoDB)
		{
			AddPlayerMDB(conn, playerControllerId);
		}
		else if(initializerScript.InitialDBType == DatabaseType.MySql)
		{
			// This coroutine thing is tricky, but the idea is that it is a delayed callback.
			// Sort of an asynchronous call in Unity if you will...
			StartCoroutine(AddPlayerSql(conn, playerControllerId));
		}

		Profiler.EndSample();
	}

	/// <summary>
	/// Called when [server remove player].
	/// </summary>
	/// <param name="conn">The connection.</param>
	/// <param name="player">The player.</param>
	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
	{
		// get the server id before that player is removed.
		short serverId = player.playerControllerId;
		base.OnServerRemovePlayer(conn, player);

		// remove that player.
		_players.Remove(serverId);
    }

	/// <summary>
	/// Add the player using MongoDB.
	/// </summary>
	/// <param name="conn">The connection.</param>
	/// <param name="playerControllerId">The player controller identifier.</param>
	private void AddPlayerMDB(NetworkConnection conn, short playerControllerId)
	{
		NetworkInitializer initializerScript = GetComponent<NetworkInitializer>();
		List<Users> users = initializerScript.GetUserList();

		// This allows us to initialize all the database data.
		PlayerInfo playerData = new PlayerInfo(users[_players.Count], initializerScript, playerCount);
		Debug.Log("Player count: "+_players.Count);
		Debug.Log("Added User: " + users[_players.Count].name + "playerId"+ playerCount);

		// Spawn the player.
		Vector3 spawnPosition = new Vector3(
			Convert.ToSingle(playerData.Location.location.x),
			Convert.ToSingle(playerData.Location.location.y),
			Convert.ToSingle(playerData.Location.location.z));

		Quaternion spawRotation = new Quaternion(
			Convert.ToSingle(playerData.Location.rotation.x),
			Convert.ToSingle(playerData.Location.rotation.y),
			Convert.ToSingle(playerData.Location.rotation.z),
			Convert.ToSingle(playerData.Location.rotation.w));

		// To do set hats and stats?
		var playerObject = (GameObject)GameObject.Instantiate(playerPrefab, spawnPosition, spawRotation);

		playerData.SetPlayerObject(playerObject);

		NetworkServer.AddPlayerForConnection(conn, playerObject, playerControllerId);

		// Debug.Log("sdfwefwef");
		_players.Add(playerCount, playerData);

		Debug.Log(_players[playerCount].ToString());
		playerCount++;
	}

	/// <summary>
	/// Called when [client disconnect]. I wish I had reference to the game object.
	/// But it appears we have to track everything manually.
	/// </summary>
	/// <param name="conn">The connection.</param>
	public override void OnClientDisconnect(NetworkConnection conn)
	{
		playerCount--;
		base.OnClientDisconnect(conn);
	}

	/// <summary>
	/// Adds the player via the SQL architecture.
	/// </summary>
	/// <param name="conn">The connection.</param>
	/// <param name="playerControllerId">The player controller identifier.</param>
	/// <returns></returns>
	private IEnumerator AddPlayerSql(NetworkConnection conn, short playerControllerId)
	{
		WWW result = new WWW("http://localhost:8080/seagull/get-player-data.php");
		yield return result;
		if (result.error != null)
		{
			Debug.LogError("Error, unable to retrieve player location!");
		}
		else
		{
			NetworkInitializer initializerScript = GetComponent<NetworkInitializer>();

			// Ideally we would use log in information to spawn the player here.
			// for now its first come first serve.
			string[] playersString = result.text.Split('|');
			string[] playerCurrent = playersString[_playersSql.Count].Split(',');

			// This represents the player data extracted from plain text html :/
			PlayerInfoSql playerData = new PlayerInfoSql(
				new Users()
				{
					user_id = Convert.ToInt32(playerCurrent[0]),
					name = playerCurrent[1]
				},
				new PlayerLocation()
				{
					location = new Location
					{
						x = Convert.ToSingle(playerCurrent[2]),
						y = Convert.ToSingle(playerCurrent[3]),
						z = Convert.ToSingle(playerCurrent[4]),
					},
					rotation = new Rotation
					{
						x = Convert.ToSingle(playerCurrent[5]),
						y = Convert.ToSingle(playerCurrent[6]),
						z = Convert.ToSingle(playerCurrent[7]),
						w = Convert.ToSingle(playerCurrent[8])
					}
				},
				playerCount, initializerScript);

			Debug.Log("Player count: " + _playersSql.Count);
			Debug.Log("Added User: " + playerData.UserId.name + "playerId" + playerCount);

			// Spawn the player.
			Vector3 spawnPosition = new Vector3(
				Convert.ToSingle(playerData.PlayerLocation.location.x),
				Convert.ToSingle(playerData.PlayerLocation.location.y),
				Convert.ToSingle(playerData.PlayerLocation.location.z));

			Quaternion spawRotation = new Quaternion(
				Convert.ToSingle(playerData.PlayerLocation.rotation.x),
				Convert.ToSingle(playerData.PlayerLocation.rotation.y),
				Convert.ToSingle(playerData.PlayerLocation.rotation.z),
				Convert.ToSingle(playerData.PlayerLocation.rotation.w));

			var playerObject = (GameObject)GameObject.Instantiate(playerPrefab, spawnPosition, spawRotation);

			playerData.SetPlayerObject(playerObject);

			NetworkServer.AddPlayerForConnection(conn, playerObject, playerControllerId);

			// adds the player to the game.
			_playersSql.Add(playerCount, playerData);

			Debug.Log(_playersSql[playerCount].ToString());
			playerCount++;
		}
	}
}