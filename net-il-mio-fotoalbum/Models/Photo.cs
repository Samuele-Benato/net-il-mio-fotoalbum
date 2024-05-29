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
        public string UserId { get; set; }
        public List<Category>? Categories { get; set; }

        public Photo() { }
        public Photo(string title, string description,  bool visible ,string userId)
        {          
            Title = title;
            Description = description;
            Visible = visible;
            UserId = userId;
        }
    }
}
