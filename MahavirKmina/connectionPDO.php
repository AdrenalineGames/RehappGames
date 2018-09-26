<?php
	$link = 'mysql:host=localhost;dbname=mahavirtest';
	$username = "root";
	$password = "";
	$tableName = 'pacientestests';
try {
    $conn = new PDO($link, $username, $password);
	//echo("Connected");
    //$conn = null;
} catch (PDOException $e) {
    print "¡Error!: " . $e->getMessage() . "<br/>";
    die();
}