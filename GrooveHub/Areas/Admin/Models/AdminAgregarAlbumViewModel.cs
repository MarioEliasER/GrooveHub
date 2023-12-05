using GrooveHub.Models.Entities;

namespace GrooveHub.Areas.Admin.Models
{
    public class AdminAgregarAlbumViewModel
    {
        public Album Album { get; set; } = new();
        public IFormFile? AlbumFile { get; set; }
    }
}
