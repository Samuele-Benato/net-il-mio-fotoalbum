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

        public static List<Photo> GetProducts()
        {
            using (PhotoContext db = new PhotoContext())
                return db.Photos.ToList();
        }

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

        public void Seed()
        {
            if(CountCategories() == 0)
            {
                InsertCategory(new Category("Wedding", null));
                InsertCategory(new Category("Black & White", null));
                InsertCategory(new Category("Nature", null));
                InsertCategory(new Category("Street Photography", null));
                InsertCategory(new Category("Portrait", null));
            }

            if(CountPhotos() == 0)
            {
                InsertPhoto(new Photo("Il Primo Sguardo", "Un momento intimo catturato durante il matrimonio di Anna e Luca, dove gli sposi si vedono per la prima volta. La luce naturale che filtra dalle finestre crea un'atmosfera magica, evidenziando l'emozione nei loro occhi.", null, true, null));
                InsertPhoto(new Photo("Ombre Metropolitane", "Una foto in bianco e nero che ritrae una strada di una grande città al tramonto. Le ombre lunghe e le silhouette dei passanti creano un gioco di contrasti che esalta l'architettura urbana e l'atmosfera frenetica della vita cittadina.", null, true, null));
                InsertPhoto(new Photo("L'Alba in Montagna", "Un'immagine mozzafiato delle montagne al sorgere del sole. I colori pastello del cielo si riflettono sulla neve immacolata, creando un panorama sereno e pacifico. Un capriolo solitario si staglia all'orizzonte, aggiungendo un tocco di vita selvaggia.", null, true, null));
                InsertPhoto(new Photo("Momenti di Vita Quotidiana", "Una fotografia che cattura l'essenza della vita di un piccolo quartiere di città. Un anziano signore siede su una panchina, osservando i bambini che giocano mentre un venditore ambulante prepara il suo carretto. L'immagine racconta storie di quotidianità e tradizione.", null, true, null));
                InsertPhoto(new Photo("Riflessi dell'Anima", "Un ritratto intenso di una giovane donna con un'espressione profonda e pensierosa. Gli occhi sono il punto focale della foto, riflettendo luce e ombre che sembrano svelare emozioni nascoste. Lo sfondo sfocato mette in risalto la figura e aggiunge un senso di mistero.", null, true, null));
            }
        }
    }
}
