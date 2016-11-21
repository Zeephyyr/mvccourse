using PhotoGallery.AppCommonCore.Contracts.Services;
using PhotoGallery.AppCommonCore.Entities;
using PhotoGallery.Common.Enums;
using PhotoGallery.Logging;
using PhotoGallery.Mapping;
using PhotoGallery.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhotoGallery.Controllers
{
    public class SearchController : Controller
    {
        private ISearchService _searchService;
        private ICustomLogger _logger;

        public SearchController(ISearchService searchService, ICustomLogger logger)
        {
            _searchService = searchService;

            _logger = logger;
        }

        [HttpPost]
        public ActionResult Search(string keyWord,string returnUrl)
        {
            _logger.Info("Search for keyword {0} from page {1}", keyWord, returnUrl);

            if (!string.IsNullOrEmpty(keyWord))
            {
                SearchResult response = _searchService.BasicSearch(keyWord);
                SearchResultViewModel result = MapperHelper.GetValue<SearchResult, SearchResultViewModel>(response);
                
                _logger.Info("Search for keyword {0} is succeeded with {1} results", keyWord,
                    (result.Albums.Count+result.Photos.Count+result.Users.Count).ToString());

                TempData["SearchResult"] = result;
                return RedirectToAction("SearchResult");
            }

            return Redirect(returnUrl);
        }

        [HttpGet]
        public ActionResult SearchResult()
        {
            SearchResultViewModel res = TempData["SearchResult"] as SearchResultViewModel ?? new SearchResultViewModel();
            return View(res);
        }

        [HttpGet]
        public ActionResult ExtendedPhotoSearch()
        {
            return View(new ExtendedPhotoSearchViewModel());
        }

        [HttpGet]
        public ActionResult ExtendedAlbumSearch()
        {
            return View(new ExtendedAlbumSearchViewModel());
        }

        [HttpGet]
        public ActionResult ExtendedUserSearch()
        {
            return View(new ExtendedUserSearchViewModel());
        }

        [HttpPost]
        public ActionResult ExtendedPhotoSearch(ExtendedPhotoSearchViewModel model)
        {
            bool resFlash;
            if (bool.TryParse(model.FlashStub, out resFlash))
            {
                model.Flash = resFlash;
            }
            else
            {
                model.Flash = null;
            }

            _logger.Info("Photo extended search requested");


            if (ModelState.IsValid)
            {
                var request = MapperHelper.GetValue<ExtendedPhotoSearchViewModel, ExtendedPhotoSearchRequest>(model);
                var response = _searchService.ExtendedPhotoSearch(request);

                List<PhotoShortViewModel> result = MapperHelper.GetValue<List<PhotoShort>, List<PhotoShortViewModel>>(response);

                TempData["Result"] = result;
                TempData["SearchType"] = ExtendedSearchType.Photo;

                _logger.Info("Photo extended search succeeded with {0} results",result.Count.ToString());

                return RedirectToAction("ExtendedSearchResult");
            }
            return View(model);
        }
        
        [HttpPost]
        public ActionResult ExtendedAlbumSearch(ExtendedAlbumSearchViewModel model)
        {
            _logger.Info("Album extended search requested");

            if (ModelState.IsValid)
            {
                var request = MapperHelper.GetValue<ExtendedAlbumSearchViewModel, ExtendedAlbumSearchRequest>(model);
                var response = _searchService.ExtendedAlbumSearch(request);
                List<AlbumShortViewModel> result = MapperHelper.GetValue<List<AlbumShort>, List<AlbumShortViewModel>>(response);

                TempData["Result"] = result;
                TempData["SearchType"] = ExtendedSearchType.Album;

                _logger.Info("Album extended search succeeded with {0} results", result.Count.ToString());

                return RedirectToAction("ExtendedSearchResult");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ExtendedUserSearch(ExtendedUserSearchViewModel model)
        {
            _logger.Info("User extended search requested");

            if (ModelState.IsValid)
            {
                var request = MapperHelper.GetValue<ExtendedUserSearchViewModel, ExtendedUserSearchRequest>(model);
                var response = _searchService.ExtendedUserSearch(request);

                List<UserShortViewModel> result = MapperHelper.GetValue<List<UserShort>, List<UserShortViewModel>>(response);

                TempData["Result"] = result;
                TempData["SearchType"] = ExtendedSearchType.User;

                _logger.Info("User extended search succeeded with {0} results", result.Count.ToString());

                return RedirectToAction("ExtendedSearchResult");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ExtendedSearchResult()
        {
            ViewBag.Result = TempData["Result"];
            ViewBag.SearchType = TempData["SearchType"];
            return View();
        }
    }
}