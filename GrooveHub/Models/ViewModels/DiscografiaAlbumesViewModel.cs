using GrooveHub.Models.Entities;

namespace GrooveHub.Models.ViewModels
{
    public class DiscografiaAlbumesViewModel
    {
        public IEnumerable<AlbumModel> Albumes { get; set; } = null!;
    }

    public class AlbumModel
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = null!;

        public DateTime FechaLanzamiento { get; set; }
    }
}
