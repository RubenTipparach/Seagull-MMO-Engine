using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Users serializable class for Mongo DB utilization.
/// </summary>
public class Users
{
    public ObjectId Id { get; set; }
    public int user_id { get; set; }
    public string name { get; set; }
    public int age { get; set; }
}


