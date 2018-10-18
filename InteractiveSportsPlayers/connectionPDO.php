<?php
	//$link = 'mysql:host=localhost;dbname=mahavirdb';
	//$username = "root";
	//$password = "";
	//$tableName = 'playerstests';
	//echo("Connecting");
	
	$link = 'mysql:host=localhost;dbname=id7534388_deportesinteractivosapp';
	$username = "id7534388_mahavir_kmina";
	$password = "K4m1N4";
	$tableName = 'playersdata';
try {
    $conn = new PDO($link, $username, $password);
	echo("Connected");
    //$conn = null;
} catch (PDOException $e) {
    echo "Error";// . $e->getMessage() . "<br/>";
    die();
}