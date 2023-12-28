use master 
go


--Microsoft SQL Server 2019 (RTM) - 15.0.2000.5 (X64)   Sep 24 2019 13:48:23   Copyright (C) 2019 Microsoft Corporation  Developer Edition (64-bit) on Windows 10 Pro 10.0 <X64> (Build 19045: ) 

create database SewaKendaraan
go

use SewaKendaraan
go

Create table Users(
Id uniqueidentifier NOT NULL PRIMARY KEY,
username varchar(20) not null,
passwords varchar(150) not null,
Roles varchar(20)
)

go

create table Kendaraan(
id uniqueidentifier not null primary key, 
names varchar(25) not null,
nopol varchar(12) ,
konsumsiBBM varchar(20),
jadwalServis datetime,
onLoan bit
)
go
create table Pemesanan(
id uniqueidentifier not null primary key,
KendaraanId uniqueidentifier null FOREIGN KEY references Kendaraan(Id),
UserId uniqueidentifier null foreign key references users(Id),
CreatedBy uniqueidentifier not null foreign key references users(Id),
createdDate datetime not null,
ApprovedBy uniqueidentifier null foreign key references users(Id),
ApprovedDate datetime null,
LastApprovedBy uniqueidentifier null foreign key references users(Id),
LastApprovedDate datetime null,
idPemesan uniqueidentifier null foreign key references users(Id),
IdDriver uniqueidentifier null foreign key references users(Id)
)
go

create table LogAplikasi(
id uniqueidentifier not null primary key,
KendaraanId uniqueidentifier null FOREIGN KEY references Kendaraan(Id),
UserId uniqueidentifier null foreign key references users(Id),
CreatedBy uniqueidentifier not null foreign key references users(Id),
createdDate datetime not null,
ApprovedBy uniqueidentifier null foreign key references users(Id),
ApprovedDate datetime null,
LastApprovedBy uniqueidentifier null foreign key references users(Id),
LastApprovedDate datetime null,
idPemesan uniqueidentifier null foreign key references users(Id),
IdDriver uniqueidentifier null foreign key references users(Id)
)

---insert users
insert into Users values (NEWID(),'Admin','test123','Admin')
insert into Users values (NEWID(),'Atasan','test123','Atasan')
insert into Users values (NEWID(),'Atasan2','test123','Atasan')
insert into Users values (NEWID(),'Atasan3','test123','Atasan')
insert into Users values (NEWID(),'Driver','test123','driver')
insert into Users values (NEWID(),'Driver2','test123','driver')
insert into Users values (NEWID(),'Driver3','test123','driver')
insert into Users values (NEWID(),'Driver4','test123','driver')
insert into Users values (NEWID(),'pegawai','test123','pegawai')
insert into Users values (NEWID(),'pegawai2','test123','pegawai')
insert into Users values (NEWID(),'pegawai3','test123','pegawai')
insert into Users values (NEWID(),'pegawai4','test123','pegawai')


--insert kendaraan 
--0 mean false for onLoan
insert into Kendaraan values (NEWID(),'Honda Vario','A1231BA','gatau',GETDATE(),0)
insert into Kendaraan values (NEWID(),'Honda Brio','B4444CD','gatau',GETDATE(),0)
insert into Kendaraan values (NEWID(),'Yamaha Lexi','D3232EF','gatau',GETDATE(),0)
insert into Kendaraan values (NEWID(),'Honda Beat','N3433AA','gatau',GETDATE(),0)

