using UnityEngine;
using System;
using System.Collections;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Collections.Generic;

public class NetworkInitializer : MonoBehaviour {

	/// <summary>
	/// The client.
	/// </summary>
	private MongoClient _client;

	/// <summary>
	/// The server.
	/// </summary>
	private MongoServer _server;

	/// <summary>
	/// The database instance.
	/// </summary>
	private MongoDatabase _db;

	/// <summary>
	/// The collection of users - probably won't be used other than for testing.
	/// </summary>
	private MongoCollection<Users> _users;

	/// <summary>
	/// The internal postion tracker. For testing that one guy. I'll have to make more trackers for other birds.
	/// </summary>
	private Transform _internalPostionTracker;

	/// <summary>
	/// Sets the track position test.
	/// </summary>
	/// <value>
	/// The track position test.
	/// </value>
	public Transform TrackPositionTest
	{
		set
		{
			_internalPostionTracker = value;
		}
	}

    /// <summary>
    /// Use this for initialization. Starts this instance.
    /// </summary>
    void Awake ()
	{
		InitializeMongoDBConnection();
		// StartCoroutine(InitializeMySQl());
    }

    /// <summary>
    /// Runs the MySql instance of the server.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitializeMySQl()
	{
		WWW result = new WWW("http://localhost:8080/seagull/index.php");
		yield return result;
		if (result.error != null)
		{
			Debug.Log("Error");
		}
		else
		{
			Debug.Log("PHP response: " + result.text);
		}
	}

    /// <summary>
    /// Initializes the mongo database instance of this intializer.
    /// </summary>
    private void InitializeMongoDBConnection()
	{
        //_client = new MongoClient(new MongoUrl("mongodb://localhost")); //27017
        _client = new MongoClient(new MongoUrl("mongodb://localhost")); //27017
        _server = _client.GetServer();

		_server.Connect();
		_db = _server.GetDatabase("seagulldb");
		_users = _db.GetCollection<Users>("Users");

		Debug.Log(string.Format("Found {0} documents.", _users.Count()));

		var jsonString = _users.FindAll().ToJson();
		var cursor = _users.FindAll();
		Debug.Log(jsonString + "!");

		foreach (var u in cursor)
		{
			Debug.Log(u.name);
		}
	}

	/// <summary>
	/// Primary update method for unity.
	/// </summary>
	void Update()
    {
		
		// Stop line for all mongo db stuff.
		if (_db == null) { return; }

		var syncPlayers = GetComponent<CustomNetworkManager>().SyncPlayers;

		// Synchronize all players. Maybe we need a timer?
		if (syncPlayers != null)
		{
			foreach (var sp in syncPlayers)
			{
				sp.Value.SynchronizePlayer();
			}
		}
    }

    /// <summary>
    /// Read database method. Only retrieve one item,
    /// designed for specific UserId based objects.
    /// </summary>
    /// <param name="dbCallback"></param>
    public T ReadOneFromDatabase<T>(string collectionName, ObjectId user_Id)
    {
        MongoCollection<T> dataCollection = _db.GetCollection<T>(collectionName);
        return dataCollection.FindOne(Query.EQ("user_Id", user_Id));
    }

    /// <summary>
    /// A more generalized callback method designed for
    /// general purpose data reads with the database instance.
    /// </summary>
    /// <param name="dbCallback"></param>
    public void ReadDatabase(Action<MongoDatabase> dbCallback)
    {
        dbCallback(_db);
    }

    /// <summary>
    /// Write to datrabase method. Might merge into one if I 
    /// can't find any additional usages.
    /// </summary>
    /// <param name="dbCallback"></param>
    public void UpdateDatabase(Action<MongoDatabase> dbCallback)
    {
        dbCallback(_db);
    }


	/// <summary>
	/// Writes the poop message.
	/// </summary>
	/// <param name="message">The message.</param>
	public void WritePoopMessage(string message)
	{
	}

	/// <summary>
	/// Get's a list of registered users on the databse.
	/// </summary>
	/// <returns></returns>
	public List<Users> GetUserList()
	{
		MongoCollection<Users> usersCollection = _db.GetCollection<Users>("Users");
		var cursor = usersCollection.FindAll();
		List<Users> usersFound = new List<Users>();

		foreach (var u in cursor)
		{
			usersFound.Add(u);
		}

		return usersFound;
	}
}
