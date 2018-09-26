<?php
	include_once 'connectionPDO.php';		
		
	$username = $_POST["usernamePost"]; //"David";
	$pass = $_POST["passPost"]; //"654";
	
	///PDO
	$add_table = "INSERT INTO $tableName (Username,Password) VALUES (?,?)";
	$add_sent = $conn->prepare($add_table);
	$add_sent->execute(array($username,$pass));
	
	if(!$add_table) echo "There was an error";
	else echo "Added";
