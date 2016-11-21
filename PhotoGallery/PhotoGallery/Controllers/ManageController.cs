using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PhotoGallery.AppCommonCore.Contracts.Services;
using PhotoGallery.Logging;
using PhotoGallery.Models;
using PhotoGallery.Common;
using PhotoGallery.Mapping;
using PhotoGallery.AppCommonCore.Entities;

namespace PhotoGallery.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private IUserService _userService;

        private ICustomLogger _logger;

        public ManageController(IUserService userService, ICustomLogger logger)
        {

            _userService = userService;

            _logger = logger;
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
            IUserService userService, ICustomLogger logger) : this(userService, logger)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.ChangeInfoSuccess ? "Info changed successfully."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            string userId = User.Identity.GetUserId();
            _logger.Info("User {0} requested a password change", userId);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                _logger.Info("User {0} succeeded with a password change");

                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        [HttpGet]
        public ActionResult ChangeUserInfo()
        {
            var request = new RequestEntity
            {
                UserId = User.Identity.GetUserId()
            };

            var response = _userService.GetUserInfo(request); ;

            ChangeUserInfoViewModel model = MapperHelper.GetValue<User, ChangeUserInfoViewModel>(response);

            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeUserInfo(ChangeUserInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                _logger.Info("User {0} requested to change info", userId);

                model.UserId = userId;

                var request = MapperHelper.GetValue<ChangeUserInfoViewModel,User>(model);

                _userService.UpdateUserInfo(request);

                return RedirectToAction("Index", new { Message = ManageMessageId.ChangeInfoSuccess });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult PremiumInfo()
        {
            return View(IsPremium());
        }

        [HttpPost]
        public ActionResult ActivatePremium()
        {
            string userId = User.Identity.GetUserId();
            _logger.Info("User {0} requested to activate premium", userId);

            _userService.ActivatePremium(userId);

            TempData["ResultMessage"] = string.Format(SuccessMessages.SuccessfullyActivatedPremium);

            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public ActionResult DeactivatePremium()
        {
            string userId = User.Identity.GetUserId();

            _logger.Info("User {0} requested to deactivate premium", userId);

            _userService.DeactivatePremium(userId);

            TempData["ResultMessage"] = string.Format(SuccessMessages.SuccessfullyDeactivatedPremium,ConfigurationElements.MaxPhotoCount,ConfigurationElements.MaxAlbumCount);

            return RedirectToAction("Index", "Home");
        }

        private bool IsPremium()
        {
            return _userService.IsPremium(User.Identity.GetUserId());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            ChangeInfoSuccess,
            Error
        }

#endregion
    }
}