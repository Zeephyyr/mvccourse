using System;
using System.Collections.Generic;
using PhotoGallery.AppCommonCore.Contracts.Services;
using PhotoGallery.AppCommonCore.Contracts.DataAccess;
using PhotoGallery.AppCommonCore.Entities;
using System.Linq;
using PhotoGallery.Mapping;

namespace PhotoGallery.Services
{
    public class AlbumService : IAlbumService
    {
        private IRepository<Album> _albumRepo;
        private IRepository<Photo> _photoRepo;
        private IRepository<User> _userRepo;

        public AlbumService(IRepository<Album> albumRepo,
            IRepository<Photo> photoRepo,IRepository<User> userRepo)
        {
            _albumRepo = albumRepo;
            _photoRepo = photoRepo;
            _userRepo = userRepo;
        }

        public void AddAlbum(Album data)
        {
            data.CreationDate = DateTime.Now;
            data.AlbumId = Guid.NewGuid().ToString();

            _albumRepo.Add(data);
        }

        public AlbumResult GetAlbum(RequestEntity data)
        {
            Album result = _albumRepo.Get(x => x.AlbumName == data.AlbumName && x.UniqueUserName == data.UniqueUserName);

            List<PhotoShort> resultPhotos = result.Photos
                .OrderBy(x => x.PhotoName)
                .Skip(data.PagingInfo.ObjectsPerPage * (data.PagingInfo.CurrentPage - 1))
                .Take(data.PagingInfo.ObjectsPerPage)
                .Select(x => new PhotoShort
                {
                    PhotoName = x.PhotoName,
                    ImageData = x.MidSizeImageData,
                    ImageMimeType = x.ImageMimeType,
                    UniqueUserName = x.UniqueUserName
                }).ToList();

            data.PagingInfo.TotalObjects = result.Photos.Count;

            AlbumInfo albumInf = MapperHelper.GetValue<Album, AlbumInfo>(result);

            if (resultPhotos!=null && resultPhotos.Count>0)
            {
                AlbumResult globalResult = new AlbumResult
                {
                    AlbumInfo = albumInf,
                    Photos = resultPhotos,
                    PagingInfo = data.PagingInfo
                };

                return globalResult;
            }

            return new AlbumResult
            {
                PagingInfo = data.PagingInfo,
                Photos = new List<PhotoShort>(),
                AlbumInfo=albumInf
            };
        }   

        public List<AlbumShort> GetAllAlbumsForUser(RequestEntity data)
        {
            List<AlbumShort> albums = _albumRepo.GetEntities(x=>x.UniqueUserName==data.UniqueUserName)
                .Select(x => new AlbumShort
                {
                    AlbumName = x.AlbumName,
                    ImageData = x.ImageData,
                    ImageMimeType = x.ImageMimeType,
                    UniqueUserName = x.UniqueUserName
                })
                .ToList(); ;

            if(albums!=null && albums.Count>0)
            {
                return albums;
            }

            return new List<AlbumShort>();
        }

        public void RemoveAlbum(RequestEntity data)
        {
            Album album = _albumRepo.Get(x => x.UniqueUserName == data.UniqueUserName && x.AlbumName == data.AlbumName);
            _albumRepo.Remove(album);
        }

        public void AddPhotoToAlbum(RequestEntity data)
        {
            Photo photoToAdd = _photoRepo.Get(x => x.PhotoName == data.PhotoName && x.UniqueUserName == data.UniqueUserName);
            Album albumToModify = _albumRepo.Get(x => x.AlbumName == data.AlbumName && x.UniqueUserName == data.UniqueUserName);

            albumToModify.Photos.Add(photoToAdd);
            _albumRepo.Update(albumToModify);
        }

        public AlbumInfo GetAlbumInfo(RequestEntity data)
        {
            AlbumInfo result = _albumRepo.GetEntities(x => x.UniqueUserName == data.UniqueUserName && x.AlbumName == data.AlbumName)
                 .Select(x => new AlbumInfo
                 {
                     CreationDate = x.CreationDate,
                     Description = x.Description,
                     AlbumName = x.AlbumName,
                     ImageData = x.ImageData,
                     ImageMimeType = x.ImageMimeType,
                     UniqueUserName = x.UniqueUserName
                 }).FirstOrDefault();
            return result;
        }

        public UpdateAlbum GetAlbumForUpdate(RequestEntity data)
        {
            UpdateAlbum album=_albumRepo.GetEntities(x=>x.UniqueUserName==data.UniqueUserName && x.AlbumName==data.AlbumName)
                .Select(x => new UpdateAlbum
                {
                    AlbumId = x.AlbumId,
                    AlbumName = x.AlbumName,
                    Description = x.Description,
                    ImageData = x.ImageData,
                    ImageMimeType = x.ImageMimeType,
                    UniqueUserName = x.UniqueUserName
                }).FirstOrDefault();
            return album;
        }

        public void UpdateAlbum(UpdateAlbum data)
        {
            Album album = _albumRepo.Get(x => x.AlbumId==data.AlbumId);
            if (album != null)
            {
                album.AlbumName = data.AlbumName;
                album.Description = data.Description;
                if (data.ImageData != null && data.ImageMimeType != null)
                {
                    album.ImageData = data.ImageData;
                    album.ImageMimeType = data.ImageMimeType;
                }
            }
            _albumRepo.Update(album);
        }

        public int GetAlbumCountForUser(RequestEntity data)
        {
            int count = _userRepo.Get(x => x.UniqueUserName == data.UniqueUserName).Albums.Count;

            return count;
        }
    }
}
