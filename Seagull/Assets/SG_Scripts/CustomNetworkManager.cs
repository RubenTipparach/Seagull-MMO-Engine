using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class CustomNetworkManager : NetworkManager
{
	/// <summary>
	/// The player spawn location.
	/// </summary>
	[SerializeField]
	Transform playerSpawnLocation;

	/// <summary>
	/// The players list.
	/// </summary>
	private Dictionary<short, PlayerInfo> _players;

	/// <summary>
	/// Provides the list to the initializer to sync the player.
	/// </summary>
	/// <value>
	/// The synchronize players.
	/// </value>
	public Dictionary<short, PlayerInfo> SyncPlayers
	{
		get
		{
			return _players;
		}
	}

	/// <summary>
	/// Called when [server ready].
	/// </summary>
	/// <param name="conn">The connection.</param>
	public override void OnServerReady(NetworkConnection conn)
	{
		_players = new Dictionary<short, PlayerInfo>();
		NetworkServer.SetClientReady(conn);
	}

	/// <summary>
	/// called when a new player is added for a client.
	/// </summary>
	/// <param name="conn">The connection.</param>
	/// <param name="playerControllerId">The player controller identifier.</param>
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		//this.numPlayers
		AddPlayerMDB(conn, playerControllerId);
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
		PlayerInfo playerData = new PlayerInfo(users[_players.Count], initializerScript);

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
		_players.Add(playerControllerId, playerData);
		Debug.Log(_players[playerControllerId].ToString());
	}
}