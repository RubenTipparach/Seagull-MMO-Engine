using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Items
{
	public ObjectId Id { get; set; }
	public Hats hats { get; set; }
	public int quantitity { get; set; }
}
