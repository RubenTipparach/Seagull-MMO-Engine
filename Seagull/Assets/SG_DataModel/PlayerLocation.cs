﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

class PlayerLocation
{
    public ObjectId Id { get; set; }
    public ObjectId user_id { get; set; }
    public Location location { get; set; }
}
