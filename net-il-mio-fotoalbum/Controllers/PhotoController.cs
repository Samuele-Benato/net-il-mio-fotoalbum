using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;

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
            return View(PhotoManager.GetPhoto(id));
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
            var photoToEdit = PhotoManager.GetPhoto(id);

            if(photoToEdit == null)
            {
               return RedirectToAction(nameof(Edit));
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

                if (PhotoManager.EditPhoto(
                id,
                data.Photo.Title,
                data.Photo.Description,               
                data.Photo.ImageFile,
                data.Photo.Visible,
                data.SelectedCategories))

                    return RedirectToAction(nameof(Index));

                else
                    return RedirectToAction(nameof(Edit));
            }
            catch
            {
                return View();
            }
        }

        // GET: PhotoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PhotoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
