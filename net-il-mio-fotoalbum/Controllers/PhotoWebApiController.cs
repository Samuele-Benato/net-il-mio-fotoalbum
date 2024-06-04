using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotoWebApiController : ControllerBase
    {
        [HttpGet("{name?}")]
        public IActionResult GetPhotos() 
        {            
            return Ok(PhotoManager.GetVisiblePhotos());
        }

        [HttpGet("{name}")]
        public IActionResult GetPhotoByName(string name) 
        {
            var photo = PhotoManager.GetPhotosByName(name);
            if (photo == null)
                return NotFound("Foto non trovata!");
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> PostContactMessage(ContactMessage message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var db = new PhotoContext())
            {
                db.ContactMessages.Add(message);
                await db.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
