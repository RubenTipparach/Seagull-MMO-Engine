using UnityEngine;
using System.Collections;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

public class NetworkInitializer : MonoBehaviour {

	private MongoClient _client;
	private MongoServer _server;
	private MongoDatabase _db;
	private MongoCollection<Users> _users;

    [SerializeField]
    Transform boxTracker;

    /// <summary>
    /// Use this for initialization. Starts this instance.
    /// </summary>
    void Start () {
		
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

    void Update()
    {
        MongoCollection<PlayerLocation> locations = _db.GetCollection<PlayerLocation>("PlayerLocation");

        // Find me the user
        IMongoQuery queryMyName = Query.EQ("name", "Ruben");
        Users cursor = _users.FindOne(queryMyName);

        // Now find the player location and update it.
        MongoDB.Driver.Builders.Update.Set("", "").Set("","");
    }

    /// <summary>
    /// Writes the poop message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void WritePoopMessage(string message)
	{
	}
}
