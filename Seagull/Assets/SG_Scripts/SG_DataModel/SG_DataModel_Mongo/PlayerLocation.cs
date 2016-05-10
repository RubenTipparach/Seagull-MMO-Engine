using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using UnityEngine;

public class PlayerLocation
{
    public ObjectId Id { get; set; }
    public ObjectId user_Id { get; set; }
    public Location location { get; set; }
	public Rotation rotation { get; set; }

	/// <summary>
	/// Synchronizes the locaiton and rotation.
	/// </summary>
	/// <param name="gameObjectTransform">The game object transform.</param>
	public void SyncLocaitonAndRotation(Transform gameObjectTransform)
	{
		location.x = gameObjectTransform.position.x;
		location.y = gameObjectTransform.position.y;
		location.z = gameObjectTransform.position.z;

		rotation.w = gameObjectTransform.rotation.w;
		rotation.x = gameObjectTransform.rotation.x;
		rotation.y = gameObjectTransform.rotation.y;
		rotation.z = gameObjectTransform.rotation.z;
    }
}

