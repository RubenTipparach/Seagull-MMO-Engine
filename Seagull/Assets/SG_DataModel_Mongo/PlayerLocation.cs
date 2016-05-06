using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

public class PlayerLocation
{
    public ObjectId Id { get; set; }
    public ObjectId user_Id { get; set; }
    public Location location { get; set; }
}

