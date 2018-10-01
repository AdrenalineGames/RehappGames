<?php
	ob_start();
	include_once 'connectionPDO.php';	
	ob_end_clean();	
		
	$username = $_POST["usernamePost"];
	$pass = $_POST["passPost"];
	
	///Classic
	//$sql = "SELECT ID, Name, Pass, MarathonScore, SkiScore, DodgeballScore, MahavirID FROM pacientes";
	//$result = mysqli_query($conn, $sql);	
	// if(mysqli_num_rows($result) > 0){
		//Show data for each row
		// while($row = mysqli_fetch_assoc($result)){
			// echo("ID: " .$row['ID'] ."|Name:" .$row['Name'] ."|Pass:" .$row['Pass'] 
			// ."|MarathonScore:" .$row['MarathonScore'] ."|SkiScore:" .$row['SkiScore'] 
			// ."|DodgeballScore:" .$row['DodgeballScore'] ."|MahavirID:" .$row['MahavirID'] .";");
		// }
	// }
	
	///PDO
	$table = "SELECT * FROM $tableName WHERE Username = ?";
	$prep_sent = $conn->prepare($table);
	$prep_sent->execute(array($username));
	$result = $prep_sent->fetch();
		
	if($result){
		if($result['Password'] == $pass){
			echo($result['ID'] ."|" .$result['Username'] ."|" .$result['Password'] 
			."|" .$result['Marathon_level'] ."|" .$result['Marathon_speed'] ."|" .$result['Marathon_steps'] 
			."|" .$result['Skiing_level'] ."|" .$result['Skiing_speed'] ."|" .$result['Dodgeball_level'] 
			."|" .$result['Save_date']);
		}
		else echo 'Wrong password';
	}
	else echo 'Player not found';
?>