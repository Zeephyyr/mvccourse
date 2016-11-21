CREATE PROCEDURE BasicAlbumSearch
	@KeyWord varchar(50)
AS
	Select al.AlbumName,al.UniqueUserName,al.ImageData,al.ImageMimeType from Albums al
	where al.AlbumName like '%'+@KeyWord+'%';
GO

CREATE PROCEDURE BasicPhotoSearch
	@KeyWord varchar(50)
AS
	Select ph.PhotoName,ph.UniqueUserName,ph.MiniatureImageData as ImageData,ph.ImageMimeType from Photos ph
	where ph.PhotoName like '%'+@KeyWord+'%'
GO


CREATE PROCEDURE BasicUserSearch
	@KeyWord varchar(50)
AS
	Select u.Name,u.UniqueUserName from UserInfos u
	where u.Name like '%'+@KeyWord+'%' or u.UniqueUserName like '%'+@KeyWord+'%'
GO

--drop procedure BasicAlbumSearch
--drop procedure BasicPhotoSearch
--drop procedure BasicUserSearch