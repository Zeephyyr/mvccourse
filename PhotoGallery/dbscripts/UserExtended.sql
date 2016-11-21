CREATE PROCEDURE ExtendedUserSearch
	@Name varchar(32) null,
	@UniqueUserName varchar(32) null,
	@Description varchar(30) null
AS
	DECLARE @sqlCommand nvarchar(max)
	DECLARE @ParameterDefinition AS NVARCHAR(max)
	DECLARE @likeName varchar(70)
	DECLARE @likeUniqueUserName varchar(70)
	DECLARE @likeDescription varchar(70)
	DECLARE @protection bit

	set	@likeName='%'+@Name+'%'
	set	@likeUniqueUserName='%'+@UniqueUserName+'%'
	set	@likeDescription='%'+@Description+'%'
	set @protection= case when 
	@Name is null and
	@UniqueUserName is null and
	@Description is null
	then 1 else 0 end
	
	if @protection=1
	begin
		return
	end
							 
	Set @sqlCommand=
	N'Select u.Name,u.UniqueUserName
	from UserInfos u
	where u.Name is not Null ' 
	+ CASE WHEN @Name IS NOT NULL THEN N' and u.Name like @likeName' ELSE N'' END 
	+ CASE WHEN @UniqueUserName IS NOT NULL THEN N' and u.UniqueUserName like @likeUniqueUserName' ELSE N'' END 
	+ CASE WHEN @Description IS NOT NULL THEN N' and u.About like @likeDescription' ELSE N'' END 

	set @ParameterDefinition=
	' 
	@likeName varchar(70),
	@likeUniqueUserName varchar(70),
	@likeDescription varchar(70)
	'

	EXECUTE sp_executesql 
	@sqlCommand,
	@ParameterDefinition,
	@likeName,
	@likeUniqueUserName,
	@likeDescription
GO

--drop procedure ExtendedUserSearch

--EXEC ExtendedUserSearch
--@Name=vlad,
--@UniqueUserName = null,
--@Description = null
