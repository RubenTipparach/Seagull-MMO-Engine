using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BirdCharacter
{
    public ObjectId Id { get; set; }
    public double ExperiencePoints { get; set; }
    public int Level { get; set; }
}

