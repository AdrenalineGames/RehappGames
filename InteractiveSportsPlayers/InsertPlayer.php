<?php
	ob_start();
	include_once 'connectionPDO.php';	
	ob_end_clean();	
		
	$username = $_POST["usernamePost"]; //"David";
	$pass = $_POST["passPost"]; //"654";
	$mahavir = $_POST["mahavirPost"]; //"654";
	
	///PDO
	$add_table = "INSERT INTO $tableName (Username,Password,Mahavir_patient) VALUES (?,?,?)";
	$add_sent = $conn->prepare($add_table);
	$add_sent->execute(array($username,$pass,$mahavir));
	
	if(!$add_table) echo "There was an error";
	else echo "Added";
