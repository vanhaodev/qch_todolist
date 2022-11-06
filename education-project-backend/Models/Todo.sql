

Create proc CreateWorkspace (
	@WorkspaceID varchar(11), @uid varchar(11), @displayname nvarchar(30), @description nvarchar(max)
)
as
	Begin
	INSERT INTO [dbo].[Workspace]
           ([WorkspaceID]
           ,[uid]
           ,[displayname]
           ,[description])
     VALUES
           (@WorkspaceID, @uid, @displayname, @description)

	end

-----------------------------------
create TABLE TodoItem(
	todoid bigint identity(1,1) primary key,
	workspaceID varchar(11) not null,
	iconid int not null default 0,
	title nvarchar(30) not null default N'Công việc mới',
	description nvarchar(max) not null default N'Mô tả công việc mới',
	deadline datetime DEFAULT '2050-1-1',
	isDeleted bit default 0,
	createdAt datetime DEFAULT SYSDATETIME(),
	updatedAt datetime DEFAULT SYSDATETIME()

	 CONSTRAINT FK_TodoItem_Workspace FOREIGN KEY (workspaceID)
    REFERENCES workspace(workspaceID)
)
go
CREATE PROC CreateTodoItem(
	@workspaceID varchar(11), @iconid int, @title nvarchar(30),
	@description nvarchar(max), @deadline datetime
)
AS
	BEGIN
		INSERT INTO [dbo].[TodoItem]
           ([workspaceID]
           ,[iconid]
           ,[title]
           ,[description]
           ,[deadline])
		VALUES
           (@workspaceID, @iconid, @title, @description, @deadline)
		
	END
--------------------------------------------
go

CREATE TABLE WorkspaceUser(
	WorkspaceUserid bigint identity(1,1) primary key NOT NULL,
	uid varchar(11) NOT NULL,
	workspaceID varchar(11) NOT NULL,
	role tinyint NOT NULL default 0,
	joinedAt datetime NOT NULL DEFAULT SYSDATETIME(),
	updatedAt datetime NOT NULL DEFAULT SYSDATETIME()

	CONSTRAINT FK_WorkspaceUser_UserAccount FOREIGN KEY (uid)
    REFERENCES UserAccount(uid),
	CONSTRAINT FK_WorkspaceUser_Workspace FOREIGN KEY (workspaceID)
    REFERENCES workspace(workspaceID)
	)

	go

Create proc AddMember(
@uid varchar(11), @workspaceid varchar(11), @role tinyint
)
AS 
	BEGIN
		INSERT INTO [dbo].[WorkspaceUser]
           ([uid]
           ,[workspaceID]
           ,[role])
		VALUES
           (@uid, @workspaceid, @role)
	END