create table lesson (
	lesson_id	uuid		primary key
	, subject 	varchar(8)	not null	
	, start_time	time     	not null
	, end_time	time    	not null
	, date_lesson	date    	not null
	, name_class	varchar(8)	not null
	, teacher_id	uuid		not null	
	, task_id	uuid		
);


