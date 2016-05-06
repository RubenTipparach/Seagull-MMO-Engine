using UnityEngine;
using System;
using System.Collections;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

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
		InitializeMongoDB();
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
    private void InitializeMongoDB()
	{
        //_client = new MongoClient(new MongoUrl("mongodb://localhost")); //27017
        _client = new MongoClient(new MongoUrl("mongodb://localhost")); //27017
        _server = _client.GetServer();

		_server.Connect();
		_db = _server.GetDatabase("seagulldb");
		_users = _db.GetCollection<Users>("Users");

		Debug.Log(string.Format("Found {0} documents.", _users.Count()));
		var filter = new Users();

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
		if (_internalPostionTracker != null && _db != null)
		{
			UpdateMDB();
		}
    }

	/// <summary>
	/// The MongoDB update method.
	/// Currently it keeps the player's brd consistent.
	/// </summary>
	private void UpdateMDB()
	{
		MongoCollection<PlayerLocation> locations = _db.GetCollection<PlayerLocation>("PlayerLocation");

		// Find me the user
		IMongoQuery queryMyName = Query.EQ("name", "Ruben");
		Users cursor = _users.FindOne(queryMyName);
		// Debug.Log(string.Format("Found User: {0}.", cursor.name));

		// Find the User Id associated with the player location.
		IMongoQuery findLocalEntry = Query.EQ("user_Id", cursor.Id);
		PlayerLocation pl = locations.FindOne(findLocalEntry);

		// Debug.Log(string.Format("Found {0} {1} {2}.", pl.location.x, pl.location.y, pl.location.z));

		// Now find the player location and update it.
		var update = MongoDB.Driver.Builders.Update
			.Set("location.x", _internalPostionTracker.position.x)
			.Set("location.y", _internalPostionTracker.position.y)
			.Set("location.z", _internalPostionTracker.position.z);

		// Run the updat call with the filter Query.
		var result = locations.Update(findLocalEntry, update);
	}

	/// <summary>
	/// Links up player position to MongoDB.
	/// </summary>
	/// <returns></returns>
	public Vector3 LinkUpPlayerPositionToMDB()
	{
		MongoCollection<PlayerLocation> locations = _db.GetCollection<PlayerLocation>("PlayerLocation");

		// Find me the user
		IMongoQuery queryMyName = Query.EQ("name", "Ruben");
		Users cursor = _users.FindOne(queryMyName);
		// Debug.Log(string.Format("Found User: {0}.", cursor.name));

		// Find the User Id associated with the player location.
		IMongoQuery findLocalEntry = Query.EQ("user_Id", cursor.Id);
		PlayerLocation p1l = locations.FindOne(findLocalEntry);
		Location p1Location = p1l.location;

		// Debug.Log(string.Format("Found {0} {1} {2}.", pl.location.x, pl.location.y, pl.location.z));
		return new Vector3(Convert.ToSingle(p1Location.x), Convert.ToSingle(p1Location.y), Convert.ToSingle(p1Location.z));
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
}
