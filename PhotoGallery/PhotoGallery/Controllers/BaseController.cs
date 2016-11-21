using Microsoft.AspNet.Identity;
using PhotoGallery.AppCommonCore.Contracts.Services;
using PhotoGallery.Logging;
using System.Web.Mvc;

namespace PhotoGallery.Controllers
{
    public abstract class BaseController : Controller
    {
        protected virtual ActionResult ErrorIfNotAnOwner(string uniqueUserName, 
            string errorType,IUserService userService,ICustomLogger logger)
        {
            string userId = User.Identity.GetUserId();

            return ErrorIfNotAnOwner(uniqueUserName, errorType, userService, userId, logger);
        }

        protected virtual ActionResult ErrorIfNotAnOwner(string uniqueUserName,
            string errorType, IUserService userService,string userId, ICustomLogger logger)
        {
            string uniqueUserNameFromDb = userService.GetUniqueUserNameById(userId);

            if (uniqueUserNameFromDb != uniqueUserName)
            {
                logger.Error(errorType);
                return View("Error", errorType);
            }
            return null;
        }
    }
}