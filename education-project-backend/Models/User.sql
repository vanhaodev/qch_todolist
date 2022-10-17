CREATE TABLE UserAccount (
    uid varchar(11) PRIMARY KEY,
    userName VARCHAR(30) UNIQUE NOT NULL,
	password NVARCHAR(512) NOT NULL,
    displayName NVARCHAR(30) NOT NULL,
    email NVARCHAR(255) UNIQUE NOT NULL,
	phoneNumber VARCHAR(30),
	avatarUrl VARCHAR(2084) NOT NULL DEFAULT 'https://file.lhu.edu.vn/me/avatarorigin/119001304.jpg',
	quote NVARCHAR(512) NOT NULL DEFAULT 'ileven in your area!!',
	birthday DATE NOT NULL DEFAULT '2001-10-25',
	isActivate BIT NOT NULL DEFAULT 1,
	banTo DATETIME NOT NULL DEFAULT '2001-10-25', --khóa tài khoản đến
	role tinyint NOT NULL DEFAULT 0,
	createdAt DATETIME NOT NULL DEFAULT SYSDATETIME(),
);
ALTER TABLE [UserAccount] ALTER COLUMN password nvarchar(512) NOT NULL;

DELETE FROM [UserAccount]
INSERT INTO [dbo].[UserAccount]
           ([userName],[password],[displayName],[email])
     VALUES
           ('nvh2001', 'nvh2001', N'Dân chơi xóm', 'vanhao.dev@gmail.com')

		  


go
CREATE PROCEDURE UserAccount_Register
	(@uid VARCHAR(11),
    @userName VARCHAR(30),
    @password NVARCHAR(512),
	@displayName NVARCHAR(30),
    @email VARCHAR (255))
AS
BEGIN
    SET NOCOUNT ON;
     
    IF EXISTS(SELECT userName FROM UserAccount WHERE userName = @userName)
    BEGIN
        SELECT -1 as 'status' --username exists already
    END
    IF EXISTS(SELECT email FROM UserAccount WHERE email = @email)
    BEGIN
        SELECT -2 as 'status' --email exists already
    END
	ELSE
    BEGIN
        INSERT INTO UserAccount
                (uid, userName, password, displayName, email)
        VALUES (@uid,
				@userName,
                @password,
				@displayName,
                @email)
        SELECT 1 as 'status', uid, userName, displayName FROM UserAccount where userName = @userName  -- Identity of Cus_SKY Scope()
    END
END
go
UserAccount_Register 'gdragon5', '1234', N'PRORORGOG ê', 'haohao6@gmail.com'

 select * from UserAccount where uid = 'UK0UQuP78U2'

 go
 create proc UserAccount_GetUID
 (@userName varchar(255))
 as
 if @userName LIKE '%@%' --nếu người dùng đăng nhập bằng email
	begin
		select uid from UserAccount where email = @userName
	end
else -- username
	begin
		select uid from UserAccount where userName = @userName
	END



go

CREATE PROCEDURE UserAccount_Login
    (@userName VARCHAR(30),
    @password VARCHAR(512))
AS
	IF EXISTS (SELECT * FROM UserAccount WHERE userName = @userName AND password = @password) 
	BEGIN
    SELECT 1 as 'status',* FROM UserAccount WHERE userName = @userName
	END
ELSE
	BEGIN
    SELECT -1 as 'status'
	END

go
alter PROC UserAccount_GetPassword (@userName VARCHAR(255))
AS 
	if @userName LIKE '%@%' --nếu người dùng đăng nhập bằng email
	begin
		IF EXISTS (SELECT * FROM UserAccount WHERE email = @userName) 
			BEGIN
			SELECT 1, password FROM UserAccount WHERE email = @userName
		END
		ELSE
			BEGIN
			SELECT -1 as 'status'
		END
	end
	else --nếu người dùng đăng nhập bằng username
		IF EXISTS (SELECT * FROM UserAccount WHERE userName = @userName) 
			BEGIN
			SELECT 1, password FROM UserAccount WHERE userName = @userName
		END
		ELSE
			BEGIN
			SELECT -1 as 'status'
		END

UserAccount_Login 'nvh2001', 'nvh2001'

alter PROCEDURE UserAccount_ChangePassword
	(@username VARCHAR(30),
	@password NVARCHAR(512),
	@newPassword NVARCHAR(512))
AS
	IF EXISTS (SELECT * FROM UserAccount WHERE userName = @userName AND password = @password) 
	BEGIN
	UPDATE UserAccount SET password = @newPassword WHERE userName = @userName
    SELECT 1
	END
ELSE
	BEGIN
    SELECT -1 as 'status'
	END

	UserAccount_ChangePassword 'nvh2001', 'nvh2001', 'newpass'


	select * from UserAccount


create Table Test_UserID(
	uid varchar(11) primary key,
	loopCount int default 0
	)	
	select count(*) from Test_UserID
	select * from Test_UserID ORDER BY NEWID ();     
	go
		sp_spaceused Test_UserID
	delete Test_UserID
	go

	insert into Test_UserID 
	(uid, loopCount) values 
	('{value}', 1)

