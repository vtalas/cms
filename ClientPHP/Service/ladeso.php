<?php 

$url = "http://localhost:62728/clientapi/c78ee05e-1115-480b-9ab7-a3ab3c0f6643/getpage/testPage_link";
//$url = "http://ladeso.aspone.cz//clientapi/c78ee05e-1115-480b-9ab7-a3ab3c0f6643/getpage/testPage_link";

$opts = array(
  'http'=>array(
    'method'=>"GET",
    'header'=>'Accept:application/json, text/plain, */*\r\n'.
    	'Accept-Charset:windows-1250,utf-8;q=0.7,*;q=0.3'
  )
);

$context = stream_context_create($opts);

$file = file_get_contents($url, false, $context);
echo $file;

?>