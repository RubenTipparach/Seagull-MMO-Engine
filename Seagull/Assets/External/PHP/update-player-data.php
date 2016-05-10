<?
	$conn = new mysqli("localhost:3306", "root", "", "seagulldb");
	
	$userId = $_POST["user_Id"];
	
	$locX = $_POST["locX"];
	$locY = $_POST["locY"];
	$locZ = $_POST["locZ"];

	$rotX = $_POST["rotX"];
	$rotY = $_POST["rotY"];
	$rotZ = $_POST["rotZ"];
	$rotW = $_POST["rotW"];
	
	// Formulate the ypdate string.
	$updatePlayerLocationSql = sprintf("
	UPDATE player_location 
	SET X = %f, Y = %f, Z = %f 
	WHERE UserId = %d;",
	$locX, $locY, $locZ, $userId);
	
	if ($conn->query($updatePlayerLocationSql) === TRUE) {
		echo "Record updated successfully";
	} else {
		echo "Error updating record: " . $conn->error;
	}

	$updatePlayerRotationSql = sprintf("
	UPDATE player_rotation
	SET X = %f, Y = %f, Z = %f, W = %f
	WHERE UserId = %d;",
	$rotX, $rotY, $rotZ, $rotW, $userId);

	if ($conn->query($updatePlayerRotationSql) === TRUE) {
		echo "Record updated successfully";
	} else {
		echo "Error updating record: " . $conn->error;
	}

	// print($updatePlayerSql);

	mysqli_close($conn);
?>