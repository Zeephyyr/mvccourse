using PhotoGallery.AppCommonCore.Entities;
using PhotoGallery.Mapping;
using PhotoGallery.Models;
using System.Collections.Generic;

namespace PhotoGallery.AppCommonCore
{
    sealed class AutoMapperInitiazlier
    {
        public void InitMapper()
        {
            List<TypeContainer> bindings = new List<TypeContainer>();

            bindings.Add(new TypeContainer(typeof(Album),typeof(AddAlbumViewModel)));

            bindings.Add(new TypeContainer(typeof(AlbumShort), typeof(AlbumShortViewModel)));

            bindings.Add(new TypeContainer(typeof(AlbumInfo), typeof(AlbumInfoViewModel)));
            bindings.Add(new TypeContainer(typeof(PhotoShort), typeof(PhotoShortViewModel)));

            bindings.Add(new TypeContainer(typeof(AlbumInfo), typeof(Album)));
            bindings.Add(new TypeContainer(typeof(AlbumResult), typeof(AlbumViewModel)));
            bindings.Add(new TypeContainer(typeof(UpdateAlbum), typeof(UpdateAlbumViewModel)));

            bindings.Add(new TypeContainer(typeof(UsersWall), typeof(UsersWallViewModel)));

            bindings.Add(new TypeContainer(typeof(Photo), typeof(PhotoViewModel)));
            bindings.Add(new TypeContainer(typeof(Photo), typeof(UploadPhotoViewModel)));
            bindings.Add(new TypeContainer(typeof(UpdatePhoto), typeof(UpdatePhotoViewModel)));

            bindings.Add(new TypeContainer(typeof(User), typeof(ChangeUserInfoViewModel)));

            bindings.Add(new TypeContainer(typeof(UserShort), typeof(UserShortViewModel)));

            bindings.Add(new TypeContainer(typeof(SearchResult), typeof(SearchResultViewModel)));
            bindings.Add(new TypeContainer(typeof(ExtendedPhotoSearchViewModel), typeof(ExtendedPhotoSearchRequest)));
            bindings.Add(new TypeContainer(typeof(ExtendedAlbumSearchViewModel), typeof(ExtendedAlbumSearchRequest)));
            bindings.Add(new TypeContainer(typeof(ExtendedUserSearchViewModel), typeof(ExtendedUserSearchRequest)));

            MapperHelper.Initalize(bindings);
        }
    }
}
