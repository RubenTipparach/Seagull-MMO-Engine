using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// This class, similar to its MongoDb counterpart handles
/// data initialization from the databse model, and handles 
/// synchronization of data between the game and database.
/// </summary>
public class PlayerInfoSql
{
	/// <summary>
	/// The User Id, used to grab all the other stuff.
	/// </summary>
	private Users _userId;

	/// <summary>
	/// Player location and rotation. Used to synchronize where the player is.
	/// </summary>
	private PlayerLocation _playerLocation;
}

