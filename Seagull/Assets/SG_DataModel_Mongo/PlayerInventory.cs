using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlayerInventory
{
    public ObjectId Id { get; set;  }
    public ObjectId user_id { get; set; }
    public Items[] items { get; set; }
}
