using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PhotoGallery.AppCommonCore.Contracts.Services;
using PhotoGallery.AppCommonCore.Entities;
using PhotoGallery.Common;
using PhotoGallery.Common.Enums;
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
    public class PhotoController : BaseController
    {
        private IPhotoService _photoService;
        private IUserService _userService;
        private IAlbumService _albumService;

        private ICustomLogger _logger;

        public PhotoController(IPhotoService photoService, IUserService userService, IAlbumService albumService,
            ICustomLogger logger)
        {
            _photoService = photoService;
            _userService = userService;
            _albumService = albumService;

            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ViewWall(string uniqueUserName = null, int offset = 1)
        {
            string actualUserIdOrName = uniqueUserName ?? User.Identity.GetUserId();
            Guid userId;

            if(Guid.TryParse(actualUserIdOrName,out userId))
            {
                actualUserIdOrName = _userService.GetUniqueUserNameById(userId.ToString());
            }
            else
            {
                if(!_userService.CheckIfUserExists(uniqueUserName))
                {
                    return View("Error", string.Format(Errors.UserNotFound, uniqueUserName));
                }
            }

            var request = new RequestEntity
            {
                UniqueUserName = actualUserIdOrName,
                PagingInfo = new PagingInfo
                {
                    ObjectsPerPage = ConfigurationElements.DefaultPageLimit,
                    CurrentPage = offset
                }
            };

            var response = _photoService.GetUsersWall(request);

            UsersWallViewModel result = MapperHelper.GetValue<UsersWall, UsersWallViewModel>(response);

            ViewBag.UniqueUserName = actualUserIdOrName;

            ViewBag.ResultMessage = TempData["ResultMessage"];

            return View(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ViewPhoto(string uniqueUserName, string photoName)
        {
            string userId = User.Identity.GetUserId();

            string currentUserName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                 currentUserName = _userService.GetUniqueUserNameById(userId);
            }

            var request = new RequestEntity
            {
                UniqueUserName = uniqueUserName,
                PhotoName = photoName,
                CurrentUserName = currentUserName
            };

            var response = _photoService.GetPhoto(request);

            PhotoViewModel photo = MapperHelper.GetValue<Photo, PhotoViewModel>(response);

            if(photo==null)
            {
                var errMsg = string.Format(Errors.PhotoNotFound, photoName, uniqueUserName);
                _logger.Error(errMsg);
                return View("Error", errMsg);
            }

            ViewBag.CurrentUniqueUserName = currentUserName;

            ViewBag.ResultMessage = TempData["ResultMessage"];

            return View(photo);
        }

        [HttpGet]
        public ActionResult UploadPhoto()
        {
            if(!_userService.UserAlloweUploadPhotos(User.Identity.GetUserId()))
            {
                _logger.Error(Errors.PhotoLimitReached);
                return View("Error", Errors.PhotoLimitReached);
            }

            return View(new UploadPhotoViewModel());
        }

        [HttpPost]
        public ActionResult UploadPhoto(UploadPhotoViewModel model, HttpPostedFileBase image = null)
        {
            if(ModelState.IsValid)
            {
                if(image!=null)
                {
                    if(image.ContentLength <  ConfigurationElements.MaxFileSize * ConfigurationElements.ModifierForMaxSize)
                    {
                        model.ImageMimeType = image.ContentType;
                        if (model.ImageMimeType.ToLower().Contains("image/jpeg"))
                        {
                            string userId = User.Identity.GetUserId();

                            _logger.Info("User {0} attempts to add a new photo", userId);

                            model.UniqueUserName = _userService.GetUniqueUserNameById(userId);

                            var request = new RequestEntity
                            {
                                PhotoName = model.PhotoName,
                                UniqueUserName = model.UniqueUserName
                            };

                            if (!_photoService.CheckIfPhotoExists(request))
                            {
                                model.MiniatureImageData = new byte[image.ContentLength];

                                image.InputStream.Read(model.MiniatureImageData, 0, image.ContentLength);
                                model.MidSizeImageData = model.MiniatureImageData;
                                model.SourceImageData = model.MiniatureImageData;

                                var uploadRequest = MapperHelper.GetValue<UploadPhotoViewModel, Photo>(model);
                                _photoService.UploadPhoto(uploadRequest);

                                _logger.Info("User {0} successfully added photo", userId);
                                TempData["ResultMessage"] = string.Format(SuccessMessages.SuccessfullyUploadedPhoto, model.PhotoName);

                                return RedirectToAction("ViewPhoto", new { uniqueUserName = model.UniqueUserName, photoName = model.PhotoName });
                            }
                            else
                            {
                                ModelState.AddModelError("AlreadyExists", "Photo with such name already exists in your collection");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("MimeType", "Wrong type, should be jpeg");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Size", "The file is too big");
                    }
                }
                else
                {
                    ModelState.AddModelError("NoPic", "There is no photo");
                }
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult RatePhoto(RequestEntity model)
        {
            if(model.CurrentUserName==model.UniqueUserName)
            {
                _logger.Error(Errors.OwnerTriesToRate);
                return null;
            }
            _photoService.Rate(model);

            model.PhotoName = model.PhotoName;

            RatingData rating=_photoService.GetRating(model);

            var response = JsonConvert.SerializeObject(rating);
            return Json(response);
        }

        [HttpPost]
        public ActionResult RemovePhoto(RemovePhotoViewModel model)
        {
            string userId = User.Identity.GetUserId();
            _logger.Info("User {0} tries to remove photo {1}", userId, model.PhotoName);

            ActionResult alternativeResult = ErrorIfNotAnOwner(model.UniqueUserName, Errors.AttemptToRemovePhoto, _userService, userId,_logger);

            if (alternativeResult == null)
            {
                alternativeResult = ErrorIfNoPhoto(model.PhotoName, model.UniqueUserName);

                if (alternativeResult == null)
                {
                    var request = new RequestEntity
                    {
                        UniqueUserName = model.UniqueUserName,
                        PhotoName = model.PhotoName
                    };

                    _photoService.RemovePhoto(request);

                    _logger.Info("Photo {0} successfully deleted", model.PhotoName);
                    TempData["ResultMessage"] = string.Format(SuccessMessages.SuccessfullyRemovedPhoto,model.PhotoName);

                    return RedirectToAction("ViewWall");
                }
            }

            return alternativeResult;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ViewFullSize(string photoName,string uniqueUserName)
        {
            return ErrorIfNoPhoto(photoName, uniqueUserName)
                ?? View(new FullSizeViewModel
                    {
                        PhotoName=photoName,
                        UniqueUserName=uniqueUserName
                    });
        }

        [AllowAnonymous]
        public FileContentResult GetImg(string photoName,string uniqueUserName, RequiredImgSize size)
        {
            var request = new RequestEntity
            {
                RequiredSize = size,
                PhotoName = photoName,
                UniqueUserName = uniqueUserName
            };

            ImageData img = _photoService.GetImgByName(request);

            return File(img.Content, img.ImageMimeType);
        }

        [HttpGet]
        public ActionResult EditPhotoPage(string photoName,string uniqueUserName,string photoId)
        {
            return ErrorIfNotAnOwner(uniqueUserName, Errors.AttemptToEditPhoto, _userService,_logger)
                    ?? ErrorIfNoPhoto(photoName,uniqueUserName)
                    ?? View(new EditPhotoViewModel
                    {
                        PhotoName = photoName,
                        UniqueUserName = uniqueUserName
                    });
                    
        }
        
        [HttpGet]
        public ActionResult AddToAlbum(string photoName,string uniqueUserName)
        {
            ActionResult alternativeResult = ErrorIfNotAnOwner(uniqueUserName,Errors.AttemptToAdAlbum, _userService,_logger);

            if (alternativeResult == null)
            {
                alternativeResult = ErrorIfNoPhoto(photoName, uniqueUserName);

                if (alternativeResult == null)
                {
                    var request = new RequestEntity
                    {
                        UniqueUserName = uniqueUserName
                    };

                    var response = _albumService.GetAllAlbumsForUser(request);

                    AllAlbumsShortViewModel result = new AllAlbumsShortViewModel();
                    result.Albums = MapperHelper.GetValue<List<AlbumShort>, List<AlbumShortViewModel>>(response);

                    ViewBag.PhotoName = photoName;
                    ViewBag.UniqueUserName = uniqueUserName;

                    return View(result);
                }
            }

            return alternativeResult;
        }

        [HttpPost]
        public ActionResult AddToAlbum(AddPhotoToAlbumViewModel model)
        {
            string userId = User.Identity.GetUserId();
            _logger.Info("photo {0} is requested to be added to album {1} by {2}", model.PhotoName, model.AlbumName, userId);

            var request = new RequestEntity
            {
                UniqueUserName = model.UniqueUserName,
                AlbumName = model.AlbumName,
                PhotoName = model.PhotoName
            };

            _albumService.AddPhotoToAlbum(request);

            _logger.Info("Successfully added photo {0} to albuem {1} both owned by {2}", model.PhotoName, model.AlbumName, userId);

            TempData["ResultMessage"] = string.Format(SuccessMessages.SuccessfullyAddedPhotoToAlbum,model.PhotoName,model.AlbumName);

            return RedirectToAction("ViewAlbum","Album",new { albumName=model.AlbumName,uniqueUserName=model.UniqueUserName });
        }

        [HttpGet]
        public ActionResult UpdatePhoto(string photoName, string uniqueUserName)
        {
            ActionResult alternativeResult = ErrorIfNotAnOwner(uniqueUserName,Errors.AttemptToUpdatePhoto, _userService,_logger);

            if(alternativeResult==null)
            {
                alternativeResult = ErrorIfNoPhoto(photoName, uniqueUserName);

                if (alternativeResult == null)
                {
                    var request = new RequestEntity
                    {
                        PhotoName = photoName,
                        UniqueUserName = uniqueUserName
                    };

                    var response = _photoService.GetPhotoForUpdate(request);

                    UpdatePhotoViewModel result = MapperHelper.GetValue<UpdatePhoto, UpdatePhotoViewModel>(response);
                    return View(result);
                }
            }
            return alternativeResult;
        }

        [HttpPost]
        public ActionResult UpdatePhoto(UpdatePhotoViewModel model)
        {
            if(ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                _logger.Info("User {0} requests to update photo {1}", userId, model.PhotoId);

                ActionResult alternativeResult = ErrorIfNotAnOwner(model.UniqueUserName, Errors.AttemptToUpdatePhoto, _userService, userId, _logger);

                if(alternativeResult==null)
                { 
                    var request = MapperHelper.GetValue<UpdatePhotoViewModel, UpdatePhoto>(model);
                    
                    _photoService.UpdatePhoto(request);

                    TempData["ResultMessage"] = string.Format(SuccessMessages.SuccessfullyUpdatedPhoto, model.PhotoName);

                    return RedirectToAction("ViewPhoto", new { photoName = model.PhotoName, uniqueUserName = model.UniqueUserName });
                }
                return alternativeResult;
            }
            return View(model);
        }

        private ActionResult ErrorIfNoPhoto(string photoName, string uniqueUserName)
        {
            var request = new RequestEntity
            {
                PhotoName = photoName,
                UniqueUserName = uniqueUserName
            };
            if (!_photoService.CheckIfPhotoExists(request))
            {
                string errMsg = string.Format(Errors.PhotoNotFound, photoName, uniqueUserName);
                _logger.Error(errMsg);
                return View("Error",errMsg);
            }
            return null;
        }
    }
}