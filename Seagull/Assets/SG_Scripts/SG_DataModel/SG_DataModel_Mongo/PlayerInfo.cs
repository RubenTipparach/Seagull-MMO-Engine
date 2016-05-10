using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// This Player Info class handles the initial data retrieval
/// and assiging the attributes from the data model to the game model.
/// It is also responsible for synchronization of any changes to 
/// the game model with the game's internal model.
/// 
/// Since this is a prototype designed to show case only a few of
/// the MongoDB/NoSql design features, the decorative stuff like player
/// stats and player inventory load into memory, but don't have any
/// affect on gameplay currently. Lots of other little 
/// features were implemented for fun.
/// </summary>
public class PlayerInfo
{
	/// <summary>
	/// The User Id, used to grab all the other stuff.
	/// </summary>
    private Users _userId;

	/// <summary>
	/// Player statistical data.
	/// </summary>
    private PlayerStats _playerStats;

	/// <summary>
	/// Player location and rotation. Used to synchronize where the player is.
	/// </summary>
    private PlayerLocation _playerLocation;

	/// <summary>
	/// Player Inventory storage.
	/// </summary>
    private PlayerInventory _playerInventory;

    /// <summary>
    /// Player's stats and attributes.
    /// </summary>
    public PlayerStats Stats { get { return _playerStats; } }

    /// <summary>
    /// Player location data.
    /// </summary>
    public PlayerLocation Location { get { return _playerLocation; } }

    /// <summary>
    /// Player inventory data.
    /// </summary>
    public PlayerInventory Inventory { get { return _playerInventory; } }
        
    /// <summary>
    /// An instance of the initializer, some sort of update loop
    /// will call method operators to get this working.
    /// </summary>
    private NetworkInitializer _initializer;

	// Unity game stuff.
	private Transform _playerTransform;

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
    /// The constructor for the main Player info class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="initializer"></param>
    public PlayerInfo(Users userId, NetworkInitializer initializer, int internalId)
    {
        _userId = userId;
        _initializer = initializer;

        // set all player variables here.
        _playerStats = GetPlayerStats(_userId.Id);
        _playerLocation = GetPlayerLocation(_userId.Id);
        _playerInventory = GetPlayerInventory(_userId.Id);

		InternalId = internalId;
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
		_playerTransform = playerObject.transform;
	}

    /// <summary>
    /// Synchronizes the player witht eh database
    /// </summary>
	/// <remarks>
	/// Ummm...maybe don't call until I got all transactions fleshed out.
	/// Sadly, not all the sync methods will work,
	/// theoretically, they would be tied to an event callback method
	/// that is triggered when some in game event happens to change the
	/// state of the data. This data will then be applied to the DB.
	/// </remarks>
    public IEnumerator SynchronizePlayer()
    {
        // SyncStats();
        // SyncInventory();
        yield return SyncLocation();
    }


    /// <summary>
    /// Assigns the player stats attribute some data.
    /// </summary>
    /// <param name="user_Id"></param>
    /// <returns></returns>
    private PlayerStats GetPlayerStats(ObjectId user_Id)
    {
        PlayerStats stats = 
            _initializer.ReadOneFromDatabase<PlayerStats>("PlayerStats", user_Id);

        if (stats == null)
        {
            throw new Exception("The player's stats were not found!");
        }

        return stats;
    }

	/// <summary>
	/// Gets the player's current location as in the database.
	/// Probably shouldn't be public.
	/// </summary>
	/// <param name="user_Id"></param>
	/// <returns></returns>
	private PlayerLocation GetPlayerLocation(ObjectId user_Id)
    {
        PlayerLocation pLocation = 
            _initializer.ReadOneFromDatabase<PlayerLocation>("PlayerLocation", user_Id);

        if (pLocation == null)
        {
            throw new Exception("The player's Location info was not found!");
        }

        return pLocation;
    }

	/// <summary>
	/// Retrieves data for the player's inventory.
	/// </summary>
	/// <param name="user_Id"></param>
	/// <returns></returns>
	private PlayerInventory GetPlayerInventory(ObjectId user_Id)
    {
        PlayerInventory pInventory =
            _initializer.ReadOneFromDatabase<PlayerInventory>("PlayerInventory", user_Id);

        if (pInventory == null)
        {
            throw new Exception("The player's Inventory info was not found!");
        }

        return pInventory;
    }

	/// <summary>
	/// Crap, there's a lot to do here!
	/// </summary>
	private void SyncStats()
	{
		// UpdateBuilder update = Update.Set("")
	}

	/// <summary>
	/// Synchronizes the location. Ok guys.. how do we this?
	/// </summary>
	private IEnumerator SyncLocation()
    {
		if (_playerTransform == null)
		{
			_initializer.gameObject.GetComponent<CustomNetworkManager>().SyncPlayers.Remove(InternalId);

		}
		else
		{
			MongoCollection<PlayerLocation> locations = null;
            _initializer.UpdateDatabase((MongoDatabase db) =>
			{
				locations = db.GetCollection<PlayerLocation>("PlayerLocation");
			});

			// To learn the mongo, one must become the mongo!
			yield return locations;

			IMongoQuery findLocalEntry = Query.EQ("user_Id", _userId.Id);
			_playerLocation.SyncLocaitonAndRotation(_playerTransform);

			UpdateBuilder update = Update
				.Set("location.x", _playerLocation.location.x)
				.Set("location.y", _playerLocation.location.y)
				.Set("location.z", _playerLocation.location.z)

				.Set("rotation.w", _playerLocation.rotation.w)
				.Set("rotation.x", _playerLocation.rotation.x)
				.Set("rotation.y", _playerLocation.rotation.y)
				.Set("rotation.z", _playerLocation.rotation.z);

			// That's a funny name. This statement updates the database with these vals.
			WriteConcernResult result = locations.Update(findLocalEntry, update);
		}
    }

	/// <summary>
	/// Theoretically we want to implement this later in the future.
	/// </summary>
    private void SyncInventory()
    {
    }

	/// <summary>
	/// The to string method, returns a string representation of this
	/// class. It shows that my queries worked! :)
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		// Consolidate stats information.
		string stats = string.Format(
@"	
	Level: {0}
	Exp: {1}
	Strength: {2}
	Agility: {3}
	Intelligence: {4}",
_playerStats.level,
_playerStats.experiencePoints,
_playerStats.attributes.strength,
_playerStats.attributes.agility,
_playerStats.attributes.intelligence);

		// Consolidate inventory.
		string inventory = "";
		foreach(var item in _playerInventory.items)
		{
			inventory += string.Format(
                @"		Item: {0} Quantity: {1} Equiped: {2}" + Environment.NewLine,
				item.hats.hatType, item.quantity, item.equiped);
		}

		// Form the one large string.
		return string.Format(
@"User Name: {0}
User ID: {1}
Location: {2},{3},{4}
Stats: {5}
Inventory:
	Items:
{6}
",
_userId.name,
_userId.Id.Pid,
_playerLocation.location.x, _playerLocation.location.y, _playerLocation.location.z,
stats,
inventory);
	}
}
