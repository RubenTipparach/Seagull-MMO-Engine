using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// This class, similar to its MongoDb counterpart handles
/// data initialization from the databse model, and handles 
/// synchronization of data between the game and database.
/// 
/// To save ourselves time, we can reuse some of MongoDB's sweet
/// data models!
/// </summary>
public class PlayerInfoSql
{
	/// <summary>
	/// The User Id, used to grab all the other stuff.
	/// </summary>
	private Users _userId;

	/// <summary>
	/// Player location and rotation. Used to synchronize where the player is.
	/// </summary>
	private PlayerLocation _playerLocation;

	/// <summary>
	/// Unity's Game Object reference.
	/// </summary>
	private GameObject _playerObject;

	/// <summary>
	/// The internal id used for synchronizing the player with server.
	/// </summary>
	public int InternalId
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the user identifier.
	/// </summary>
	/// <value>
	/// The user identifier.
	/// </value>
	public Users UserId
	{
		get
		{
			return _userId;
		}
	}

	/// <summary>
	/// Gets the player location.
	/// </summary>
	/// <value>
	/// The player location.
	/// </value>
	public PlayerLocation PlayerLocation
	{
		get
		{
			return _playerLocation;
		}
	}

	private NetworkInitializer _initializer;

	/// <summary>
	/// Initializes a new instance of the <see cref="PlayerInfoSql"/> class.
	/// </summary>
	/// <param name="userId">The user identifier.</param>
	/// <param name="initializer">The initializer.</param>
	public PlayerInfoSql(Users userId, PlayerLocation location, int internalId, NetworkInitializer initializer)
	{
		_userId = userId;
		_playerLocation = location;
		_initializer = initializer;
    }

	/// <summary>
	/// A reference to the instantiated player.
	/// set any other game attributes here. We'll use those changes
	/// to allow us to switch back and forth between Data Model and Game Models.
	/// </summary>
	/// <param name="playerObject"></param>
	public void SetPlayerObject(GameObject playerObject)
	{
		_playerObject = playerObject;
	}

	/// <summary>
	/// Synchronizes the player.
	/// </summary>
	/// <remarks>
	/// MUST USE COROUTINE!!!!! This will be an asynchronous call.
	/// </remarks>
	public IEnumerator SynchronizePlayer()
	{
		yield return SyncLocation();
	}

	/// <summary>
	/// Synchronizes the location.
	/// </summary>
	private IEnumerator SyncLocation()
	{
		if (_playerObject == null)
		{
			_initializer.gameObject.GetComponent<CustomNetworkManager>().SyncPlayers.Remove(InternalId);
		}
		else
		{
			_playerLocation.SyncLocaitonAndRotation(_playerObject.transform);

			// Put update code here.
			WWWForm form = new WWWForm();
			form.AddField("user_Id", _userId.user_id);

			form.AddField("locX", Convert.ToString(_playerLocation.location.x));
			form.AddField("locY", Convert.ToString(_playerLocation.location.y));
			form.AddField("locZ", Convert.ToString(_playerLocation.location.z));

			form.AddField("rotX", Convert.ToString(_playerLocation.rotation.x));
			form.AddField("rotY", Convert.ToString(_playerLocation.rotation.y));
			form.AddField("rotZ", Convert.ToString(_playerLocation.rotation.z));
			form.AddField("rotW", Convert.ToString(_playerLocation.rotation.w));

			WWW result = new WWW("http://localhost:8080/seagull/update-player-data.php", form);
			yield return result;

			if (result.error != null)
			{
				Debug.LogError("Error, unable to update player location!");
			}
			else
			{
				// Debug.Log(result.text);
			}
		}
	}

	/// <summary>
	/// Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>
	/// A <see cref="System.String" /> that represents this instance.
	/// </returns>
	public override string ToString()
	{
		// Form the one large string.
		return string.Format(
@"User Name: {0}
User ID: {1}
Location: {2},{3},{4}

",
_userId.name,
_userId.Id.Pid,
_playerLocation.location.x, _playerLocation.location.y, _playerLocation.location.z);
	}
}

