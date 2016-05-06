using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlayerInfo
{
    private Users _userId;

    private PlayerStats _playerStats;

    private PlayerLocation _playerLocation;

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

    /// <summary>
    /// The constructor for the main Player info class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="initializer"></param>
    public PlayerInfo(Users userId, NetworkInitializer initializer)
    {
        _userId = userId;
        _initializer = initializer;

        // set all player variables here.
        _playerStats = GetPlayerStats(_userId.Id);
        _playerLocation = GetPlayerLocation(_userId.Id);
        _playerInventory = GetPlayerInventory(_userId.Id);
    }

    /// <summary>
    /// Synchronizes the player witht eh database
    /// </summary>
    public void SynchronizePlayer()
    {
        SyncStats();
        SyncInventory();
        SyncLocation();
    }


    /// <summary>
    /// Assigns the player stats attribute some data.
    /// </summary>
    /// <param name="user_Id"></param>
    /// <returns></returns>
    public PlayerStats GetPlayerStats(ObjectId user_Id)
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
    public PlayerLocation GetPlayerLocation(ObjectId user_Id)
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
    public PlayerInventory GetPlayerInventory(ObjectId user_Id)
    {
        PlayerInventory pInventory =
            _initializer.ReadOneFromDatabase<PlayerInventory>("PlayerInventory", user_Id);

        if (pInventory == null)
        {
            throw new Exception("The player's Inventory info was not found!");
        }

        return pInventory;
    }

    private void SyncStats()
    {

    }

    private void SyncLocation()
    {

    }

    private void SyncInventory()
    {

    }
}
