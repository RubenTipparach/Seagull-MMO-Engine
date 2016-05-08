using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlayerStats
{
	public ObjectId Id { get; set; }
	public ObjectId user_Id { get; set; }

	public int experiencePoints { get; set; }
	public int level { get; set; }
	public Attributes attributes { get; set; } // Had to pluralize, Attribute is already a C# class.
}