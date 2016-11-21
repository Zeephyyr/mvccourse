using PhotoGallery.AppCommonCore.Entities;
using System.Collections.Generic;

namespace PhotoGallery.AppCommonCore.Contracts.Services
{
    public interface IAlbumService
    {
        void AddAlbum(Album data);

        AlbumResult GetAlbum(RequestEntity data);

        List<AlbumShort> GetAllAlbumsForUser(RequestEntity data);

        void RemoveAlbum(RequestEntity data);

        void AddPhotoToAlbum(RequestEntity data);

        AlbumInfo GetAlbumInfo(RequestEntity data);
    
        UpdateAlbum GetAlbumForUpdate(RequestEntity data);

        void UpdateAlbum(UpdateAlbum data);
    }
}
