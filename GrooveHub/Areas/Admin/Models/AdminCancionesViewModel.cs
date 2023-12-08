using GrooveHub.Models.ViewModels;

namespace GrooveHub.Areas.Admin.Models
{
    public class AdminCancionesViewModel
    {
        public IEnumerable<AdminCancionesModel> Canciones { get; set; } = null!;
    }

    public class AdminCancionesModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public DateTime AñoLanzamiento { get; set; }

        public TimeOnly Duracion { get; set; }

        public int IdAlbum { get; set; }
        public string NombreAlbum { get; set; } = null!;
    }
}
