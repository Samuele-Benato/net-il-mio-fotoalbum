namespace net_il_mio_fotoalbum.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[]? ImageFile { get; set; }
        public string? ImgSrc => ImageFile != null ? $"data:image/png;base64,{Convert.ToBase64String(ImageFile)}" : "";
        public bool Visible {  get; set; }
        public List<Category>? Categories { get; set; }

        public Photo() { }
        public Photo(string title, string description, byte[]? imageFile, bool visible, List<Category>? categories)
        {          
            Title = title;
            Description = description;
            ImageFile = imageFile;
            Visible = visible;
            Categories = categories;
        }
    }
}
