using GrooveHub.Models.ViewModels;

namespace GrooveHub.Areas.Admin.Models
{
    public class AdminAlbumesViewModel
    {
        public IEnumerable<AdminAlbumModel> Albumes { get; set; } = null!;
    }

    public class AdminAlbumModel
    {
        public int Id { get; set; }
        public string NombreAlbum { get; set; } = null!;
    }
}
