Create Table Users
(
Id Int Primary Key Identity,
Name Nvarchar(50) Not null Unique,
Password Nvarchar(50) not null
)
Go

Create Table UserPermissions
(
UserId Int Not null References Users(Id) On Delete Cascade,
PermissionId Int Not null,
Primary Key (UserId, PermissionId)
)
