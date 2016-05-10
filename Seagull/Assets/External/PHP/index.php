<?
	$conn = new mysqli("localhost:3306", "root", "", "seagulldb");


    print("<br><h3>List of availible Users in database.</h3>");
	$getAiNameSql = "SELECT * FROM seagulldb.user;";
	$result = mysqli_query($conn, $getAiNameSql);

	while($row = mysqli_fetch_assoc($result))
	{
		// %u number, %s string
		print("<br><b>Name </b>".$row["Name"]. ",<b>Age </b>".$row["Age"]);
	}

    mysqli_close($conn);
?>