using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

public class Location
{
    public ObjectId Id { get; set; }
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }
}

