using PhotoGallery.AppCommonCore.Contracts.DataAccess;
using PhotoGallery.AppCommonCore.Contracts.Services;
using PhotoGallery.AppCommonCore.Entities;
using System.Collections.Generic;

namespace PhotoGallery.Services
{
    public class SearchService :ISearchService
    {
        private ISearchRepository _repo;

        public SearchService (ISearchRepository repo)
        {
            _repo = repo;
        }
        
        public SearchResult BasicSearch(string keyWord)
        {
            return _repo.Search(keyWord);
        }

        public List<PhotoShort> ExtendedPhotoSearch(ExtendedPhotoSearchRequest data)
        {
            List<PhotoShort> photos = _repo.ExtendedPhotoSearch(data);

            return photos;
        }

        public List<AlbumShort> ExtendedAlbumSearch(ExtendedAlbumSearchRequest data)
        {
            List<AlbumShort> albums = _repo.ExtendedAlbumSearch(data);

            return albums;
        }

        public List<UserShort> ExtendedUserSearch(ExtendedUserSearchRequest data)
        {
            List<UserShort> users = _repo.ExtendedUserSearch(data);

            return users;
        }
    }
}
