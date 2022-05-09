use PC_08;

create table Job (
ID int primary key identity(1,1) not null,
Name varchar(50) not null);

create table Employee (
ID int primary key identity(1,1) not null,
Username varchar(50) not null,
Password varchar(50)  not null,
Name varchar(100) not null,
Email varchar(100) not null,
Address varchar(200) not null,
DateOfBirth date not null,
JobId int not null,
Photo image not null,
foreign key (jobId) references job(id));

create table Customer (
ID int primary key identity(1,1) not null,
Name varchar(50) not null,
NIK varchar(50),
Email varchar(50),
Gender char(1),
PhoneNumber varchar(20),
Age int,
DateOfBirth date);

create table RoomType (
ID int primary key identity(1,1) not null,
Name varchar(50) not null,
Capacity int not null,
RoomPrice int not null,
Photo Image not null);

create table Room (
ID int primary key identity(1,1),
RoomTypeID int not null,
RoomNumber Varchar(50) not null,
RoomFloor varchar(50) not null,
Description text,
status varchar(50),
foreign key (roomTypeID) references roomType(id));

create table FoodsAndDrinks (
ID int primary key identity(1,1),
Name varchar(50) not null,
Type char(1),
Price int not null,
Photo Image not null,);

create table Item (
ID int primary key identity(1,1),
Name varchar(50) not null,
RequestPrice int not null,
CompensationFee int);

create table ItemStatus (
ID int primary key identity(1,1),
Name varchar(50) not null);

create table Reservation (
ID int primary key identity(1,1),
DateTime datetime not null,
EmployeeID int not null,
CustomerID int not null,
BookingCode varchar(6) not null,
foreign key(employeeID) references employee(id),
foreign key(customerID) references customer(id));

create table ReservationRoom (
ID int primary key identity(1,1),
ReservationID int not null,
RoomID int not null,
StartDateTime datetime not null,
DurationNights int not null,
RoomPrice int not null,
CheckInDateTime datetime not null,
CheckOutDateTime datetime not null,
foreign key(reservationId) references reservation(id),
foreign key(roomId) references room(id));

create table ReservationCheckOut (
ID int primary key identity(1,1),
ReservationRoomID int not null,
ItemID int not null,
ItemStatusID int not null,
Qty int,
TotalCharge int not null,
foreign key (reservationRoomId) references reservationRoom(id),
foreign key (itemId) references item(id),
foreign key (itemStatusID) references itemstatus(id));

create table ReservationRequestItem (
ID int primary key identity(1,1),
ReservationRoomID int not null,
ItemID int not null,
Qty int not null,
TotalPrice int not null,
foreign key (reservationRoomId) references reservationroom(id),
foreign key (itemId) references item(id));

create table FDCheckOut (
ID int primary key identity(1,1),
ReservationRoomID int not null,
FDID int not null,
Qty int,
TotalPrice int,
EmployeeID int not null,
foreign key (reservationRoomID) references reservationRoom(id),
foreign key (FDID) references foodsAndDrinks(id),
foreign key (EmployeeID) references Employee(id));