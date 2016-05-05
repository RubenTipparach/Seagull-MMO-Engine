using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{

	[SerializeField]
	Transform playerSpawnLocation;

	// called when a new player is added for a client
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		AddPlayerMDB(conn, playerControllerId);
    }

	// Add the player using MongoDB
	private void AddPlayerMDB(NetworkConnection conn, short playerControllerId)
	{
		NetworkInitializer initializerScript = GetComponent<NetworkInitializer>();
		Vector3 spawnPosition = initializerScript.LinkUpPlayerPositionToMDB();
		var player = (GameObject)GameObject.Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
		initializerScript.TrackPositionTest = player.transform;

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		// Debug.Log("sdfwefwef");
	}
}