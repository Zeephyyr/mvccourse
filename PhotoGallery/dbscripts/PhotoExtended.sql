CREATE PROCEDURE ExtendedPhotoSearch
	@PhotoName varchar(32) null,
	@UniqueUserName varchar(32) null,
	@Description varchar(30) null,
	@Diaphragm int null,
	@CameraModel varchar(30) null,
	@Flash bit null,
	@ISO varchar(20) null,
	@LensFocus int null,
	@ShutterSpeed int null,
	@Place varchar(50) null
AS
	DECLARE @sqlCommand nvarchar(max)
	DECLARE @ParameterDefinition AS NVARCHAR(max)
	DECLARE @likePhotoName varchar(70)
	DECLARE @likeUniqueUserName varchar(70)
	DECLARE @likeDescription varchar(70)
	DECLARE @likeCameraModel varchar(70)
	DECLARE @likeISO varchar(70)
	DECLARE @likePlace varchar(70)
	DECLARE @protection bit

	set	@likePhotoName='%'+@PhotoName+'%'
	set	@likeUniqueUserName='%'+@UniqueUserName+'%'
	set	@likeDescription='%'+@Description+'%'
	set	@likeCameraModel='%'+@CameraModel+'%'
	set	@likeISO='%'+@ISO+'%'
	set	@likePlace='%'+@Place+'%'
	set @protection= case when 
	@PhotoName is null and
	@UniqueUserName is null and
	@Description is null and
	@Diaphragm is null and
	@CameraModel is null and
	@Flash is null and
	@ISO is null and
	@LensFocus is null and
	@ShutterSpeed is null and
	@Place is null then 1 else 0 end
								 
	
	Set @sqlCommand=
	case when @protection=1 
	then 'select top 0 * from photos' 
	else
	N'Select ph.PhotoName,ph.UniqueUserName,ph.MiniatureImageData as ImageData,ph.ImageMimeType
	from Photos ph
	where ph.PhotoName is not Null ' 
	+ CASE WHEN @PhotoName IS NOT NULL THEN N' and ph.PhotoName like @likePhotoName' ELSE N'' END 
	+ CASE WHEN @UniqueUserName IS NOT NULL THEN N' and ph.UniqueUserName like @likeUniqueUserName' ELSE N'' END 
	+ CASE WHEN @Description IS NOT NULL THEN N' and ph.Description like @likeDescription' ELSE N'' END 
	+ CASE WHEN @Diaphragm IS NOT NULL OR @Diaphragm!=0 THEN N' and ph.Diaphragm = @Diaphragm' ELSE N'' END 
	+ CASE WHEN @CameraModel IS NOT NULL THEN N' and ph.CameraModel like @likeCameraModel' ELSE N'' END 
	+ CASE WHEN @Flash IS NOT NULL THEN N' and ph.Flash = @Flash' ELSE N'' END 
	+ CASE WHEN @ISO IS NOT NULL THEN N' and ph.ISO like @likeISO' ELSE N'' END 
	+ CASE WHEN @LensFocus IS NOT NULL THEN N' and ph.LensFocus = @LensFocus' ELSE N'' END
	+ CASE WHEN @ShutterSpeed IS NOT NULL THEN N' and ph.ShutterSpeed = @ShutterSpeed' ELSE N'' END 
	+ CASE WHEN @Place IS NOT NULL THEN N' and ph.Place like @likePlace' ELSE N'' END
	END

	set @ParameterDefinition=
	' 
	@likePhotoName varchar(70),
	@likeUniqueUserName varchar(70),
	@likeDescription varchar(70),
	@Diaphragm int,
	@likeCameraModel varchar(70),
	@Flash bit,
	@likeISO varchar(70),
	@LensFocus int,
	@ShutterSpeed int,
	@likePlace varchar(70)
	'

	EXECUTE sp_executesql 
	@sqlCommand,
	@ParameterDefinition,
	@likePhotoName,
	@likeUniqueUserName,
	@likeDescription,
	@Diaphragm,
	@likeCameraModel,
	@Flash,
	@likeISO,
	@LensFocus,
	@ShutterSpeed,
	@likePlace
GO

--drop procedure ExtendedPhotoSearch

--EXEC ExtendedPhotoSearch
--@PhotoName=null,
--@UniqueUserName = null,
--@Description = null,
--@Diaphragm = null,
--@CameraModel = null,
--@Flash = null,
--@ISO = null,
--@LensFocus = null,
--@ShutterSpeed = null,
--@Place = null