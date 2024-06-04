using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Data
{
    public class PhotoManager
    {
        public static int CountPhotos()
        {
            using PhotoContext db = new PhotoContext();
            return db.Photos.Count();
        }
        public static int CountCategories()
        {
            using PhotoContext db = new PhotoContext();
            return db.Categories.Count();
        }

        public static List<Photo> GetPhotos()
        {
            using (PhotoContext db = new PhotoContext())
                return db.Photos.ToList();
        }

        public static List<Photo> GetVisiblePhotos()
        {
            using (PhotoContext db = new PhotoContext())
            {
                // Filtra solo le foto con Visible impostato a true
                List<Photo> visiblePhotos = db.Photos.Where(p => p.Visible).ToList();
                return visiblePhotos;
            }
        }



        ////BONUS
        //public static List<Photo> GetPhotosByUser(string userId)
        //{
        //    using (var db = new PhotoContext())
        //    {
        //        return db.Photos.Where(p => p.UserId == userId).ToList();
        //    }
        //}

        public static List<Category> GetCategories()
        {
            using (PhotoContext db = new PhotoContext())
                return db.Categories.ToList();
        }

        public static Photo GetPhoto(int id, bool includereferences = true)
        {
            using (PhotoContext db = new PhotoContext())
                if (includereferences)
                    return db.Photos.Where(r => r.Id == id).Include(c => c.Categories).FirstOrDefault();

            using (PhotoContext db = new PhotoContext())
                return db.Photos.FirstOrDefault(p => p.Id == id);
        }

        public static List<Photo> GetPhotosByName(string name)
        {
            using PhotoContext db = new PhotoContext();

            // Rendo la ricerca case-insensitive convertendo tutto in minuscole
            return db.Photos.Where(x => x.Title.ToLower().Contains(name.ToLower())).ToList();
        }

        public static void InsertPhoto(Photo photo, List<string> SelectedCategories = null)
        {
            using PhotoContext db = new PhotoContext();
            if (SelectedCategories != null)
            {
                photo.Categories = new List<Category>();

                foreach (var categoryId in SelectedCategories)
                {
                    int id = int.Parse(categoryId);
                    var category = db.Categories.FirstOrDefault(i => i.Id == id);
                    photo.Categories.Add(category);
                }
            }
            
            db.Photos.Add(photo);
            db.SaveChanges();
        }

        public static void InsertCategory(Category category)
        {
            using PhotoContext db = new PhotoContext();
            db.Categories.Add(category);
            db.SaveChanges();
        }

        public static bool EditPhoto(int id, string title, string description, byte[] imageFile, bool visible, List<string> categories)
        {
            using PhotoContext db = new PhotoContext();
            var photo = db.Photos.Where(x => x.Id == id).Include(x => x.Categories).FirstOrDefault();

            if (photo == null)
                return false;

            photo.Title = title;
            photo.Description = description;
            photo.Visible = visible;
            photo.ImageFile = imageFile;

            photo.Categories.Clear();
            if (categories != null)
            {
                foreach (var category in categories)
                {
                    int categoryId = int.Parse(category);
                    var categoryFromDb = db.Categories.FirstOrDefault(x => x.Id == categoryId);
                    photo.Categories.Add(categoryFromDb);
                }
            }

            db.SaveChanges();

            return true;
        }

        public static bool DeletePhoto(int id)
        {
            using PhotoContext db = new PhotoContext();
            var photo = db.Photos.FirstOrDefault(p => p.Id == id );

            if (photo == null)
                return false;

            db.Photos.Remove(photo);
            db.SaveChanges();

            return true;
        }

        public static bool HidePhoto(int id)
        {
            using PhotoContext db = new PhotoContext();
            var photo = db.Photos.FirstOrDefault(p => p.Id == id);

            if (photo == null)
                return false;

            photo.Visible = false;
            db.SaveChanges();

            return true;
        }

        public static void Seed()
        {
          
            if(CountPhotos() == 0)
            {
                InsertPhoto(new Photo("Il Primo Sguardo", "Un momento intimo catturato durante il matrimonio di Anna e Luca, dove gli sposi si vedono per la prima volta. La luce naturale che filtra dalle finestre crea un'atmosfera magica, evidenziando l'emozione nei loro occhi.", true));
                InsertPhoto(new Photo("Ombre Metropolitane", "Una foto in bianco e nero che ritrae una strada di una grande città al tramonto. Le ombre lunghe e le silhouette dei passanti creano un gioco di contrasti che esalta l'architettura urbana e l'atmosfera frenetica della vita cittadina.", true));
                InsertPhoto(new Photo("L'Alba in Montagna", "Un'immagine mozzafiato delle montagne al sorgere del sole. I colori pastello del cielo si riflettono sulla neve immacolata, creando un panorama sereno e pacifico. Un capriolo solitario si staglia all'orizzonte, aggiungendo un tocco di vita selvaggia.", true));
                InsertPhoto(new Photo("Momenti di Vita Quotidiana", "Una fotografia che cattura l'essenza della vita di un piccolo quartiere di città. Un anziano signore siede su una panchina, osservando i bambini che giocano mentre un venditore ambulante prepara il suo carretto. L'immagine racconta storie di quotidianità e tradizione.", true));
                InsertPhoto(new Photo("Riflessi dell'Anima", "Un ritratto intenso di una giovane donna con un'espressione profonda e pensierosa. Gli occhi sono il punto focale della foto, riflettendo luce e ombre che sembrano svelare emozioni nascoste. Lo sfondo sfocato mette in risalto la figura e aggiunge un senso di mistero.", true));
            }

            if (CountCategories() == 0)
            {
                InsertCategory(new Category("Wedding"));
                InsertCategory(new Category("Black & White"));
                InsertCategory(new Category("Nature"));
                InsertCategory(new Category("Street Photography"));
                InsertCategory(new Category("Portrait"));
            }
        }
    }
}
