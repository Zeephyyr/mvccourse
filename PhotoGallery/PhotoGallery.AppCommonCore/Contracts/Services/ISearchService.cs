using PhotoGallery.AppCommonCore.Entities;
using System.Collections.Generic;

namespace PhotoGallery.AppCommonCore.Contracts.Services
{
    public interface ISearchService
    {
        SearchResult BasicSearch(string keyWord);

        List<PhotoShort> ExtendedPhotoSearch(ExtendedPhotoSearchRequest data);

        List<AlbumShort> ExtendedAlbumSearch(ExtendedAlbumSearchRequest data);

        List<UserShort> ExtendedUserSearch(ExtendedUserSearchRequest data);
    }
}
