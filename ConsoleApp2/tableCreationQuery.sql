create table Teacher(
	TeacherId int not null primary key,
	TeacherName varchar(256) not null,
	Surname varchar(256) not null,
	Sex varchar(16) not null,
	Subject varchar(32) not null
);

create table Pupil(
	PupilId int not null primary key,
	PupilName varchar(256) not null,
	Surname varchar(256) not null,
	Sex varchar(16) not null,
	class varchar(32) not null
);

create table TeacherPupil(
	Id int not null primary key,
	TeacherId int not null foreign key references Teacher(TeacherId),
	PupilId int not null foreign key references Pupil(PupilId)
);

