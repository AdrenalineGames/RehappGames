<?php
	$servername = "localhost";
	$server_username = "root";
	$dbPassword = "";
	$dbName = "mahavirtest";
	
	//Make Connection
	$conn = new mysqli($servername,$server_username,$dbPassword,$dbName);
	//Check connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	else{
		//echo("Connection succes");
		//echo "<br>";
	}