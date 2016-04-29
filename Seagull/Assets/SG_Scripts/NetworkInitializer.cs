using UnityEngine;
using System.Collections;
using MongoDB.Bson;
using MongoDB.Driver;

public class NetworkInitializer : MonoBehaviour {

	public class Users
	{
		public ObjectId Id { get; set; }
		public int user_id { get; set; }
		public string name { get; set; }
		public int age { get; set; }
	}

	private MongoClient _client;
	private MongoServer _server;
	private MongoDatabase _db;
	private MongoCollection<Users> _users;

	// Use this for initialization
	void Start () {
		
		InitializeMongoDB();
		StartCoroutine(InitializeMySQl());
    }

	IEnumerator InitializeMySQl()
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

	void InitializeMongoDB()
	{
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

	public void WritePoopMessage(string message)
	{

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
