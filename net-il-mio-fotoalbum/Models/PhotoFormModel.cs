using Microsoft.AspNetCore.Mvc.Rendering;
using net_il_mio_fotoalbum.Data;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoFormModel
    {
        public Photo Photo { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public List<string>? SelectedCategories { get; set; }
        public IFormFile? ImageFormFile { get; set; }

        public PhotoFormModel() { }

        public void CreateCategories()
        {
            Categories = new List<SelectListItem>();
            SelectedCategories = new List<string>();
            var categoriesFromDB = PhotoManager.GetCategories();
            foreach (var category in categoriesFromDB)
            {
                bool isSelected = Photo.Categories?.Any(i => i.Id == category.Id) == true;
                Categories.Add(new SelectListItem() // lista degli elementi selezionabili
                {
                    Text = category.Title, // Testo visualizzato
                    Value = category.Id.ToString(), // SelectListItem vuole una generica stringa, non un int
                    Selected = isSelected // es. ingrediente1, ingrediente5, ingrediente9
                });
                if (isSelected)
                    SelectedCategories.Add(category.Id.ToString()); // lista degli elementi selezionati
            }
        }

        public byte[] SetImageFileFromFormFile()
        {
            if (ImageFormFile == null)
                return null;

            using var stream = new MemoryStream();
            ImageFormFile?.CopyTo(stream);
            Photo.ImageFile = stream.ToArray();

            return Photo.ImageFile;
        }
    }
}
