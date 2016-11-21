using PhotoGallery.AppCommonCore.Entities;

namespace PhotoGallery.AppCommonCore.Contracts.Services
{
    public interface IPhotoService
    {
        void UploadPhoto(Photo data);

        void UpdatePhoto(UpdatePhoto data);

        Photo GetPhoto(RequestEntity data);

        void RemovePhoto(RequestEntity data);

        UsersWall GetUsersWall(RequestEntity data);

        void Rate(RequestEntity data);

        ImageData GetImgByName(RequestEntity data);

        int GetPhotoCountForUser(RequestEntity data);

        UpdatePhoto GetPhotoForUpdate(RequestEntity data);

        bool CheckIfPhotoExists(RequestEntity data);

        RatingData GetRating(RequestEntity data);
    }
}
