CREATE PROCEDURE ExtendedAlbumSearch
	@AlbumName varchar(32) null,
	@UniqueUserName varchar(32) null,
	@Description varchar(30) null
AS
	DECLARE @sqlCommand nvarchar(max)
	DECLARE @ParameterDefinition AS NVARCHAR(max)
	DECLARE @likeAlbumName varchar(70)
	DECLARE @likeUniqueUserName varchar(70)
	DECLARE @likeDescription varchar(70)
	DECLARE @protection bit

	set	@likeAlbumName='%'+@AlbumName+'%'
	set	@likeUniqueUserName='%'+@UniqueUserName+'%'
	set	@likeDescription='%'+@Description+'%'
	set @protection= case when 
	@AlbumName is null and
	@UniqueUserName is null and
	@Description is null
	then 1 else 0 end
	
	if @protection=1
	begin
		return
	end
							 
	Set @sqlCommand=
	N'Select al.AlbumName,al.UniqueUserName,al.ImageData,al.ImageMimeType
	from Albums al
	where al.AlbumName is not Null ' 
	+ CASE WHEN @AlbumName IS NOT NULL THEN N' and al.AlbumName like @likeAlbumName' ELSE N'' END 
	+ CASE WHEN @UniqueUserName IS NOT NULL THEN N' and al.UniqueUserName like @likeUniqueUserName' ELSE N'' END 
	+ CASE WHEN @Description IS NOT NULL THEN N' and al.Description like @likeDescription' ELSE N'' END 

	set @ParameterDefinition=
	' 
	@likeAlbumName varchar(70),
	@likeUniqueUserName varchar(70),
	@likeDescription varchar(70)
	'

	EXECUTE sp_executesql 
	@sqlCommand,
	@ParameterDefinition,
	@likeAlbumName,
	@likeUniqueUserName,
	@likeDescription
GO

--drop procedure ExtendedAlbumSearch

--EXEC ExtendedAlbumSearch
--@AlbumName=null,
--@UniqueUserName = null,
--@Description = null
