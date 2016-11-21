using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using PhotoGallery.AppCommonCore.Contracts.DataAccess;
using PhotoGallery.AppCommonCore.Contracts.Services;
using PhotoGallery.AppCommonCore.Entities;
using PhotoGallery.Common.Enums;
using System.Linq;

namespace PhotoGallery.Services
{
    public class PhotoService :IPhotoService
    {
        private IRepository<Photo> _photoRepo;
        private IRepository<User> _userRepo;

        public PhotoService (IRepository<Photo> photoRepo, IRepository<User> userRepo)
        {
            _photoRepo = photoRepo;
            _userRepo = userRepo;
        }

        public void UploadPhoto(Photo data)
        {
            data.MiniatureImageData = CutImage(data.MiniatureImageData, RequiredImgSize.Mini);
            data.MidSizeImageData = CutImage(data.MidSizeImageData, RequiredImgSize.Mid);

            data.PhotoId = Guid.NewGuid().ToString();

            _photoRepo.Add(data);
        }

        public void UpdatePhoto(UpdatePhoto data)
        {
            Photo photo = _photoRepo.Get(x => x.PhotoId == data.PhotoId);

            if (photo != null)
            {
                photo.PhotoName = data.PhotoName;
                photo.Description = data.Description;
                photo.CameraModel = data.CameraModel;
                photo.Diaphragm = data.Diaphragm;
                photo.Flash = data.Flash;
                photo.ISO = data.ISO;
                photo.LensFocus = data.LensFocus;
                photo.Place = data.Place;
                photo.ShutterSpeed = data.ShutterSpeed;
                photo.CreationDate = data.CreationDate;
            }

            _photoRepo.Update(photo);
        }

        public UpdatePhoto GetPhotoForUpdate(RequestEntity data)
        {
            UpdatePhoto res = _photoRepo.GetEntities(x=>x.UniqueUserName==data.UniqueUserName && x.PhotoName==data.PhotoName)
                .Select(x => new UpdatePhoto
                {
                    CameraModel = x.CameraModel,
                    UniqueUserName = x.UniqueUserName,
                    CreationDate = x.CreationDate,
                    Description = x.Description,
                    ShutterSpeed = x.ShutterSpeed,
                    ISO = x.ISO,
                    Diaphragm = x.Diaphragm,
                    Flash = x.Flash,
                    LensFocus = x.LensFocus,
                    PhotoId = x.PhotoId,
                    PhotoName = x.PhotoName,
                    Place = x.Place
                }).FirstOrDefault();
            return res;
        }

        public Photo GetPhoto(RequestEntity data)
        {
            Photo photo = _photoRepo.Get(x => x.UniqueUserName == data.UniqueUserName && x.PhotoName == data.PhotoName);

            if (photo == null)
                return null;

            photo.IsRatedByCurrent = !string.IsNullOrEmpty(data.CurrentUserName) ?
                _photoRepo.Get(x=>x.UniqueUserName == data.UniqueUserName && x.PhotoName==data.PhotoName)
                .LikesFrom.Any(x=>x.UniqueUserName==data.CurrentUserName):
                false;

            return photo;
        }

        public void RemovePhoto(RequestEntity data)
        {
            Photo photo = _photoRepo.Get(x => x.UniqueUserName == data.UniqueUserName && x.PhotoName == data.PhotoName);
            _photoRepo.Remove(photo);
        }

        public UsersWall GetUsersWall(RequestEntity data)
        {
            List<PhotoShort> photos = _photoRepo.GetEntities(x=>x.UniqueUserName==data.UniqueUserName)
                .Select(x => new PhotoShort
                {
                    PhotoName = x.PhotoName,
                    UniqueUserName = x.UniqueUserName,
                    ImageData = x.MiniatureImageData,
                    ImageMimeType = x.ImageMimeType
                })
                .OrderBy(x => x.PhotoName)
                .Skip(data.PagingInfo.ObjectsPerPage * (data.PagingInfo.CurrentPage - 1))
                .Take(data.PagingInfo.ObjectsPerPage).ToList();


            int count = _userRepo.Get(x => x.UniqueUserName == data.UniqueUserName).Photos.Count;
            data.PagingInfo.TotalObjects = count;

            if(photos != null&& photos.Count>0)
            {
                UsersWall result = new UsersWall
                {
                    Photos = photos,
                    PagingInfo = data.PagingInfo
                };
                return result;
            }
            return new UsersWall {
                PagingInfo=data.PagingInfo,
                Photos = new List<PhotoShort>()
            };
        }

        public void Rate(RequestEntity data)
        {
            Photo photo = _photoRepo.Get(x => x.UniqueUserName == data.UniqueUserName && x.PhotoName == data.PhotoName);

            int index = photo.LikesFrom.ToList().FindIndex(x => x.UniqueUserName == data.CurrentUserName);

            if (index == 0)
            {
                photo.LikesFrom.Remove(_userRepo.Get(x=>x.UniqueUserName==data.CurrentUserName));
                photo.Rating--;
            }
            else
            {
                photo.LikesFrom.Add(_userRepo.Get(x => x.UniqueUserName == data.CurrentUserName));
                photo.Rating++;
            }

            _photoRepo.Update(photo);
        }

        public ImageData GetImgByName(RequestEntity data)
        {
            ImageData image = _photoRepo.GetEntities(x=>x.UniqueUserName==data.UniqueUserName && x.PhotoName==data.PhotoName)
                 .Select(x => new ImageData
                 {
                     Content =
                        data.RequiredSize == RequiredImgSize.Mini ? x.MiniatureImageData :
                        data.RequiredSize == RequiredImgSize.Mid ? x.MidSizeImageData :
                        x.SourceImageData,
                     ImageMimeType = x.ImageMimeType
                 }).FirstOrDefault();
            return image;
        }

        public bool CheckIfPhotoExists(RequestEntity data)
        {
            Photo photo = _photoRepo.Get(x => x.UniqueUserName == data.UniqueUserName && x.PhotoName == data.PhotoName);
            return photo!=null;
        }

        public RatingData GetRating(RequestEntity data)
        {
            Photo photo = _photoRepo.Get(x => x.UniqueUserName == data.UniqueUserName && x.PhotoName == data.PhotoName);
            RatingData rating = new RatingData
            {
                Rating = photo.Rating,
                IsRated = photo.LikesFrom.Any(x => x.UniqueUserName == data.CurrentUserName)
            };
            return rating;
        }

        public int GetPhotoCountForUser(RequestEntity data)
        {
            int count = _userRepo.Get(x => x.UniqueUserName == data.UniqueUserName).Photos.Count;
            return count;
        }

        private byte[] CutImage(byte[] imageData,RequiredImgSize size)
        {
            Bitmap image;
            using (var ms = new MemoryStream(imageData))
            {
                image = new Bitmap(ms);
            }

            int imageSourceWidth = image.Width;
            int imageSourceHeight = image.Height;
            float modifier;
            int imageMaxWidth;
            int imageMaxHeight;

            if(size== RequiredImgSize.Mini)
            {
                imageMaxWidth = 400;
                imageMaxHeight = 400;
            }
            else
            {
                imageMaxWidth = 600;
                imageMaxHeight = 800;
            }

            
            if (imageSourceWidth > imageSourceHeight)
            {
                modifier = (float)imageSourceWidth / imageMaxWidth;
            }
            else
            {
                modifier = (float)imageSourceHeight / imageMaxHeight;
            }
            image = ResizeImage(image, (int)(imageSourceWidth / modifier), (int)(imageSourceHeight / modifier));

            return ImageToByte(image);
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}
