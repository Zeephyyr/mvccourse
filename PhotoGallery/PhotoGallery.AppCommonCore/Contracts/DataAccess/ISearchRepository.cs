using PhotoGallery.AppCommonCore.Entities;
using System.Collections.Generic;

namespace PhotoGallery.AppCommonCore.Contracts.DataAccess
{
    public interface ISearchRepository
    {
        SearchResult Search(string keyWord);

        List<PhotoShort> ExtendedPhotoSearch(ExtendedPhotoSearchRequest data);

        List<AlbumShort> ExtendedAlbumSearch(ExtendedAlbumSearchRequest data);

        List<UserShort> ExtendedUserSearch(ExtendedUserSearchRequest data);
    }
}
