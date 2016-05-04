using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

public class Location
{
    public ObjectId Id { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
}

