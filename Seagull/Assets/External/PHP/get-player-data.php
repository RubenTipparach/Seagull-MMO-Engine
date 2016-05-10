<?
	$conn = new mysqli("localhost:3306", "root", "", "seagulldb");
	$getAiNameSql = "
	SELECT u.UserId, u.Name,
		loc.X AS locX, loc.Y AS locY, loc.Z AS locZ,
		rot.X AS rotX, rot.Y AS rotY, rot.Z AS rotZ, rot.W AS rotW
	FROM user u 
		INNER JOIN player_location loc
			ON u.UserId = loc.UserId
		INNER JOIN player_rotation rot
			ON u.UserId = rot.UserId;";

	$result = mysqli_query($conn, $getAiNameSql);
	$isFirst = true;
	while($row = mysqli_fetch_assoc($result))
	{
		if(!$isFirst)
		{
			print("|");
		}
		$isFirst = false;
		// %u number, %s string
		print($row["UserId"].",".$row["Name"].",".
			$row["locX"].",".$row["locY"].",".$row["locZ"].",".
			$row["rotX"].",".$row["rotY"].",".$row["rotZ"].",".$row["rotW"]);

	}

    mysqli_close($conn);
?>