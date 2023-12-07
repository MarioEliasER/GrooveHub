using GrooveHub.Models.Entities;

namespace GrooveHub.Areas.Admin.Models
{
    public class AdminAgregarCancionViewModel
    {
        public IEnumerable<AdminAlbumModel> Albumes { get; set; } = null!;
        public Cancion Cancion { get; set; } = new();
    }
}
