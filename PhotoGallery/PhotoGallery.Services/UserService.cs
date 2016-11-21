using PhotoGallery.AppCommonCore.Contracts.DataAccess;
using PhotoGallery.AppCommonCore.Contracts.Services;
using PhotoGallery.Common;
using PhotoGallery.AppCommonCore.Entities;

namespace PhotoGallery.Services
{
    public class UserService : IUserService
    {
        private IRepository<User> _userRepo;

        public UserService(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public void AddUserInfo(User data)
        {
            _userRepo.Add(data);
        }

        public User GetUserInfo(RequestEntity data)
        {
            User user = _userRepo.Get(x => x.UserId == data.UserId);
            return user;
        }

        public void UpdateUserInfo(User data)
        {
            User user = _userRepo.Get(x => x.UserId == data.UserId);

            if (user != null)
            {
                user.Name = data.Name;
                user.About = data.About;
            }

            _userRepo.Update(user);
        }

        public string GetUniqueUserNameById(string userId)
        {
            return _userRepo.Get(x=>x.UserId==userId).UniqueUserName;
        }

        public bool CheckIfUserExists(string uniqueUserName)
        {
            User user = _userRepo.Get(x => x.UniqueUserName == uniqueUserName);
            return user!=null;
        }

        public bool UserAlloweUploadPhotos(string userId)
        {
            User user = _userRepo.Get(x => x.UserId == userId);
            return user.Photos.Count < ConfigurationElements.MaxPhotoCount;
        }

        public bool UserAlloweUploadAlbums(string userId)
        {
            User user = _userRepo.Get(x => x.UserId == userId);
            return user.Albums.Count < ConfigurationElements.MaxAlbumCount;
        }

        public void ActivatePremium(string userId)
        {
            User user= _userRepo.Get(x => x.UserId == userId);
            user.Role = RoleList.Premium;

            _userRepo.Update(user);
        }

        public void DeactivatePremium(string userId)
        {
            User user = _userRepo.Get(x => x.UserId == userId);
            user.Role = RoleList.Common;

            _userRepo.Update(user);
        }

        public bool IsPremium(string userId)
        {
            User user = _userRepo.Get(x => x.UserId == userId);
            return user.Role== RoleList.Premium;
        }
    }
}
