using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Security.Claims;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {
        // GET: PhotoController
        public ActionResult Index()
        {
            return View(PhotoManager.GetPhotos());
        }

        // GET: PhotoController/Details/5
        public ActionResult Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var photo = PhotoManager.GetPhoto(id, userId);
            if (photo == null && !User.IsInRole("SuperAdmin"))
            {
                return Unauthorized();
            }
            return View(photo);
        }

        // GET: PhotoController/Create
        public ActionResult Create()
        {
            PhotoFormModel model = new PhotoFormModel(new Photo());
            
            model.CreateCategories();
            return View(model);
        }

        // POST: PhotoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhotoFormModel data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    data.CreateCategories();
                    return View("Create", data);
                }

                data.Photo.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                data.SetImageFileFromFormFile();

                PhotoManager.InsertPhoto(data.Photo, data.SelectedCategories);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PhotoController/Edit/5
        public ActionResult Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var photoToEdit = PhotoManager.GetPhoto(id, userId);

            if (photoToEdit == null && !User.IsInRole("SuperAdmin"))
            {
                return Unauthorized();
            }

            PhotoFormModel model = new PhotoFormModel(photoToEdit);
            model.CreateCategories();
            return View(model);
        }

        // POST: PhotoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PhotoFormModel data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    data.CreateCategories();
                    return View("Edit", data);
                }

                data.SetImageFileFromFormFile();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (PhotoManager.EditPhoto(
                    id,
                    userId,
                    data.Photo.Title,
                    data.Photo.Description,
                    data.Photo.ImageFile,
                    data.Photo.Visible,
                    data.SelectedCategories))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Edit));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: PhotoController/Delete/5
        public ActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var photo = PhotoManager.GetPhoto(id, userId);
            if (photo == null && !User.IsInRole("SuperAdmin"))
            {
                return Unauthorized();
            }
            return View(photo);
        }

        // POST: PhotoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!PhotoManager.DeletePhoto(id, userId) && !User.IsInRole("SuperAdmin"))
                {
                    return Unauthorized();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Hide(int id)
        {
            if (!PhotoManager.HidePhoto(id))
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
