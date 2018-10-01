<?php
	ob_start();
	include_once 'connectionPDO.php';	
	ob_end_clean();		
		
	$username = $_POST["usernamePost"];
	$marathonLevel = $_POST["marathonLevelPost"];
	$marathonSpeed = $_POST["marathonSpeedPost"];
	$marathonSteps = $_POST["marathonStepsPost"];
	$skiingLevel = $_POST["skiingLevelPost"];
	$skiingSpeed = $_POST["skiingSpeedPost"];
	$dodgeballLevel = $_POST["dodgeballLevelPost"];
	$saveDate = $_POST["saveDatePost"];
	
	$sql_edit = "UPDATE $tableName SET Marathon_level = ?, Marathon_speed = ?, Marathon_steps = ?,
						Skiing_level = ?, Skiing_speed = ?, Dodgeball_level = ?, Save_date = ?
						WHERE Username = ?";
	$sent_edit = $conn->prepare($sql_edit);
	$sent_edit->execute(array($marathonLevel,$marathonSpeed,$marathonSteps,$skiingLevel,
								$skiingSpeed,$dodgeballLevel,$saveDate,$username));
								
								
	if(!$sql_edit) echo "There was an error";
	else echo "Saved";