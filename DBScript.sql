use master;
go

create database CPDB
go

use CPDB
go

create table TimeInterval
(
	IntervalCode nvarchar(2) not null primary key,
	IntervalDescription nvarchar(15) not null
)

create table Categories
(
	Categorycode nvarchar(2) not null primary key,
	CategoryDescription nvarchar(30) not null
)

create table Gender
(
	GenderName nvarchar(10) not null primary key,
	Genderdescription nvarchar(30) not null
)

--create table [Role]
--(
--	[Role] nvarchar(10) not null primary key,
--	RoleDescription nvarchar(30) not null
--)

create table Users
(
	IDUser integer primary key identity(1,1),
	FIO nvarchar(30) not null,
	UserAge integer not null,
	Gender nvarchar(10) not null foreign key references Gender(GenderName),
	UserImage varbinary(max) null,
	UserPhone nvarchar(20) not null,
	UserVK nvarchar(20) null,
	UserEmail nvarchar(20) null,
	[Login] nvarchar(12) not null,
	HashPass nvarchar(100) not null
)

create table Autopark
(
	CarID integer not null primary key identity(1,1),
	CarName nvarchar(20) not null,
	CarImage varbinary(max) null,
	Categorycode nvarchar(2) not null foreign key references Categories(Categorycode),
	CarYear integer not null
)

create table Instructors
(
	InstructorID integer not null primary key identity(1,1),
	FIO nvarchar(30) not null,
	InstructorImage varbinary(max) null,
	Gender nvarchar(10) foreign key references Gender(GenderName),
	CarID integer foreign key references Autopark(CarID),
	[Login] nvarchar(20) not null,
	[Password] nvarchar(max) not null
)

create table FeedBacks
(
	AuthorID integer not null foreign key references Users(IDUser),
	InstructorID integer not null foreign key references Instructors(InstructorID),
	Msg nvarchar(100) not null
)

create table TimeTable
(
	ClassID integer not null primary key identity(1,1),
	DateOfClass date not null,
	IntervalCode nvarchar(2) not null foreign key references TimeInterval(IntervalCode),
	UserID integer not null foreign key references Users(IDUser),
	InstructorID integer not null foreign key references Instructors(InstructorID)
)