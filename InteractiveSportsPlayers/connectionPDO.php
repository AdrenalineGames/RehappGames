<?php
	$link = 'mysql:host=localhost;dbname=mahavirdb';
	$username = "root";
	$password = "";
	$tableName = 'playerstests';
	//echo("Connecting");
try {
    $conn = new PDO($link, $username, $password);
	echo("Connected");
    //$conn = null;
} catch (PDOException $e) {
    echo "Error";// . $e->getMessage() . "<br/>";
    die();
}