using Microsoft.AspNet.Identity;
using PhotoGallery.AppCommonCore.Contracts.Services;
using PhotoGallery.AppCommonCore.Entities;
using PhotoGallery.Common;
using PhotoGallery.Logging;
using PhotoGallery.Mapping;
using PhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace PhotoGallery.Controllers
{
    [Authorize]
    public class AlbumController : BaseController
    {
        private IAlbumService _albumService;
        private IUserService _userService;

        private ICustomLogger _logger;

        public AlbumController(IAlbumService albumService, IUserService userService, ICustomLogger customLogger)
        {
            _albumService = albumService;
            _userService = userService;

            _logger = customLogger;
        }

        [HttpGet]
        public ActionResult AddAlbum()
        {
            if (!_userService.UserAlloweUploadAlbums(User.Identity.GetUserId()))
            {
                _logger.Info(Errors.AlbumLimitReached);
                return View("Error", Errors.AlbumLimitReached );
            }

            return View(new AddAlbumViewModel());
        }

        [HttpPost]
        public ActionResult AddAlbum(AddAlbumViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    model.ImageMimeType = image.ContentType;
                    model.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(model.ImageData, 0, image.ContentLength);

                    string userId = User.Identity.GetUserId();
                    _logger.Info("User {0} requested to add a new album", userId);

                    model.UniqueUserName = _userService.GetUniqueUserNameById(userId);

                    var album = MapperHelper.GetValue<AddAlbumViewModel, Album>(model);
                    _albumService.AddAlbum(album);

                    TempData["ResultMessage"] = string.Format(SuccessMessages.SuccessfullyCreatedAlbum, model.AlbumName);

                    return RedirectToAction("ViewAlbum", new { albumName=model.AlbumName,uniqueUserName=model.UniqueUserName });
                }
                else
                {
                    ModelState.AddModelError("NoPic", "There is no photo");
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult UserAlbums(string uniqueUserName = null)
        {
            string actualUserIdOrName = uniqueUserName ?? User.Identity.GetUserId();
            Guid userId;

            if (Guid.TryParse(actualUserIdOrName, out userId))
            {
                actualUserIdOrName = _userService.GetUniqueUserNameById(userId.ToString());
            }

            var request = new RequestEntity
            {
                UniqueUserName = actualUserIdOrName
            };

            var response = _albumService.GetAllAlbumsForUser(request);

            AllAlbumsShortViewModel result = new AllAlbumsShortViewModel();
            result.Albums = MapperHelper.GetValue<List<AlbumShort>, List<AlbumShortViewModel>>(response);

            ViewBag.ResultMessage = TempData["ResultMessage"];

            return View(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ViewAlbum(string albumName,string uniqueUserName, int offset = 1)
        {
            var request = new RequestEntity
            {
                UniqueUserName = uniqueUserName,
                AlbumName = albumName,
                PagingInfo = new PagingInfo
                {
                    ObjectsPerPage = ConfigurationElements.DefaultPageLimit,
                    CurrentPage = offset
                }
            };

            var response = _albumService.GetAlbum(request);
            AlbumViewModel result = MapperHelper.GetValue<AlbumResult, AlbumViewModel>(response);

            string userId = User.Identity.GetUserId();
            if(!string.IsNullOrEmpty(userId))
            {
                string currentUniqueUserName = _userService.GetUniqueUserNameById(userId);

                ViewBag.CurrentUniqueUserName = currentUniqueUserName;
            }

            ViewBag.ResultMessage = TempData["ResultMessage"];

            return View(result);
        }

        [HttpGet]
        public ActionResult EditAlbum(string albumName,string uniqueUserName,string albumId)
        {
            ActionResult alternativeResult = ErrorIfNotAnOwner(uniqueUserName, Errors.AttemptToEditAlbum,_userService,_logger);

            if(alternativeResult==null)
            {
                EditAlbumViewModel model = new EditAlbumViewModel
                {
                    AlbumName = albumName,
                    UniqueUserName = uniqueUserName
                };

                return View(model);
            }

            return alternativeResult;
        }

        [HttpGet]
        public ActionResult UpdateAlbum(string albumName, string uniqueUserName)
        {
            ActionResult alternativeResult = ErrorIfNotAnOwner(uniqueUserName, Errors.AttemptToUpdateAlbum, _userService,_logger);

            if (alternativeResult == null)
            {
                var request = new RequestEntity
                {
                    AlbumName = albumName,
                    UniqueUserName = uniqueUserName
                };

                var response = _albumService.GetAlbumForUpdate(request);

                UpdateAlbumViewModel result = MapperHelper.GetValue<UpdateAlbum, UpdateAlbumViewModel>(response);
                return View(result);
            }

            return alternativeResult;
        }

        [HttpPost]
        public ActionResult UpdateAlbum(UpdateAlbumViewModel model, HttpPostedFileBase image = null)
        {
            if(ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                _logger.Info("User {0} requested to update album {1}", userId, model.AlbumId);

                ActionResult alternativeResult = ErrorIfNotAnOwner(model.UniqueUserName, Errors.AttemptToRemoveAlbum, _userService, userId, _logger);

                if (alternativeResult==null)
                {
                    if (image != null)
                    {
                        model.ImageMimeType = image.ContentType;
                        model.ImageData = new byte[image.ContentLength];
                        image.InputStream.Read(model.ImageData, 0, image.ContentLength);
                    }

                    UpdateAlbum request = MapperHelper.GetValue<UpdateAlbumViewModel, UpdateAlbum>(model);

                    _albumService.UpdateAlbum(request);

                    _logger.Info("Album {0} successfully updated", model.AlbumId);
                    TempData["ResultMessage"] = string.Format(SuccessMessages.SuccessfullyUpdatedAlbum, model.AlbumName);

                    return RedirectToAction("ViewAlbum", new { albumName = model.AlbumName, uniqueUserName = model.UniqueUserName });
                }

                return alternativeResult;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult RemoveAlbum(RemoveAlbumViewModel model)
        {
            string userId = User.Identity.GetUserId();
            _logger.Info("User {0} requested to delete album {1}", userId, model.AlbumName);

            ActionResult alternativeResult = ErrorIfNotAnOwner(model.UniqueUserName, Errors.AttemptToRemoveAlbum, _userService,userId,_logger);

            if(alternativeResult==null)
            {
                var request = new RequestEntity
                {
                    UniqueUserName = model.UniqueUserName,
                    AlbumName = model.AlbumName
                };
                _albumService.RemoveAlbum(request);

                _logger.Info("Album {0} successfully deleted", model.AlbumName);
                TempData["ResultMessage"] = string.Format(SuccessMessages.SuccessfullyRemovedAlbum, model.AlbumName);

                return RedirectToAction("UserAlbums");
            }

            return alternativeResult;
        }
    }
}