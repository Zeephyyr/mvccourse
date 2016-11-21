using PhotoGallery.AppCommonCore.Entities;

namespace PhotoGallery.AppCommonCore.Contracts.Services
{
    public interface IUserService
    {
        void AddUserInfo(User data);

        void UpdateUserInfo(User data);

        User GetUserInfo(RequestEntity data);

        bool UserAlloweUploadPhotos(string userId);

        bool UserAlloweUploadAlbums(string userId);

        void ActivatePremium(string userId);

        void DeactivatePremium(string userId);

        bool IsPremium(string userId);

        #region utility

        string GetUniqueUserNameById(string userId);

        bool CheckIfUserExists(string uniqueUserName);

        #endregion
    }
}
