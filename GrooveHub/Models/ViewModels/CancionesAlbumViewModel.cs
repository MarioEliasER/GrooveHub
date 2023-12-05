namespace GrooveHub.Models.ViewModels
{
    public class CancionesAlbumViewModel
    {
        public IEnumerable<CancionesAlbumModel> CancionesAlbumes { get; set; } = null!;
    }

    public class CancionesAlbumModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public DateTime AñoLanzamiento { get; set; }

        public TimeOnly Duracion { get; set; }

        public int IdAlbum { get; set; }
    }
}
