using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Message
{
	public ObjectId Id { get; set; }
	public ObjectId user_Id { get; set; }
	public string leftByUser { get; set; }
	public string message { get; set; }
	public Location location { get; set; }
	public Rotation rotation { get; set; }
}

