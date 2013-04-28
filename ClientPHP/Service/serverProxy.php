<?php 
require  'libs.php';

$url = "http://localhost:62728/clientapi/c78ee05e-1115-480b-9ab7-a3ab3c0f6643";
//$url = "http://ladeso.aspone.cz//clientapi/c78ee05e-1115-480b-9ab7-a3ab3c0f6643/getpage/testPage_link";

function resolve($baseurl, $action, $id=null)
{
	$baseurl = $baseurl."/".$action;
	if ($id != null) {
		$baseurl = $baseurl."/".$id;
	}
	return $baseurl;
}

function getContent($url)
{
	$opts = array(
		'http'=>array(
			'method'=>"GET",
			'header'=>'Accept:application/json, text/plain, */*\r\n'.
			'Accept-Charset:windows-1250,utf-8;q=0.7,*;q=0.3'
			)
		);

	$context = stream_context_create($opts);

	if (!$file = @file_get_contents($url, false, $context)) {
		throw new Exception("Error Processing Request", 1);
	}
	return $file;
}

try {
	$action = isset($_GET["action"]) ? $_GET["action"] : null ;
	$id = isset($_GET["id"]) ? $_GET["id"] : null ;
	$u = resolve($url,$action, $id);
	echo getContent($u);
}
catch(Exception $ex) {
	echo ExceptionToJson($ex, error_get_last());

}

?>