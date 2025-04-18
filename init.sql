create table public."Lessons" (
 "Id"				uuid					primary key
 , "Subject"		text					not null
 , "StartTime"	time without time zone	not null
 , "EndTime"		time without time zone	not null
 , "Date"			date					not null
 , "ClassName"	text					not null
 , "UserId"		uuid					not null
 , "TaskID"		uuid					
);