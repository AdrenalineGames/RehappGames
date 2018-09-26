<?php
	include_once 'connectionPDO.php';
		
	$username = $_POST["usernamePost"];
	$pass = $_POST["passPost"];
	
	///PDO
	$sent = "SELECT * FROM $tableName WHERE Username = ?";
	$prep_sent = $conn->prepare($sent);
	$prep_sent->execute(array($username));
	$result = $prep_sent->fetch();
	
	if($result){
		if($result['Pass'] == $pass){
			echo "Login succes";
		} else{
			echo "Wrong Password";
		}
	}else {
		echo "User not found";
	}