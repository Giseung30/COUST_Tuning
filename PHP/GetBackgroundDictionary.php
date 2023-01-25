<?php
include_once "config.php";
$conn = mysqli_connect(DB_HOST, DB_USER, DB_PASSWORD, DB_NAME) or die("connection failed"); //DB 연결 시도
mysqli_query($conn, "Set Names UTF8"); //UTP8 설정

$columns = array(); //컬렁명 배열
$query = mysqli_query($conn, "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Unity_BackgroundDictionary'"); //컬럼명 저장
while($row = mysqli_fetch_assoc($query)) //결과가 존재하면
  array_push($columns, $row["COLUMN_NAME"]); //컬럼명 배열에 추가

$query = mysqli_query($conn, "Select * From Unity_BackgroundDictionary"); //쿼리 결과 저장
$result = array(); //결과 배열
while($row = mysqli_fetch_assoc($query)) //결과가 존재하면
{
  $rowArray = array(); //행 배열
  for($i = 0; $i < count($columns); ++$i) //컬럼의 개수만큼 반복
    $rowArray[$columns[$i]] = $row[$columns[$i]]; //행 배열에 컬럼 추가

  array_push($result, $rowArray); //결과 배열에 행 배열 추가
}

echo json_encode($result, JSON_UNESCAPED_UNICODE);

mysqli_close($conn); //연결 해제
?>
