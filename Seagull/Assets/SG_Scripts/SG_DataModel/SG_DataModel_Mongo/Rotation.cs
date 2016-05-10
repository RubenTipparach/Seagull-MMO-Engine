using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Rotation
{
	public ObjectId Id { get; set; }
	public double w { get; set; }
	public double x { get; set; }
	public double y { get; set; }
	public double z { get; set; }
}
