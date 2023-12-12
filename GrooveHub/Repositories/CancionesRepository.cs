using GrooveHub.Repositories;
using GrooveHub.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrooveHub.Repositories
{
    public class CancionesRepository : Repository<Cancion>
    {
        public CancionesRepository(GroovehubContext context) : base(context)
        {
        }

        public IEnumerable<Cancion> GetCancionesByAlbum(string albumId)
        {
            return Context.Cancion
                .Include(x=> x.IdAlbumNavigation)
                .Where(x=>x.IdAlbumNavigation.TituloAlbum ==  albumId).OrderBy(x=>x.Id);
        }

        public override IEnumerable<Cancion> GetAll()
        {
            return Context.Cancion
                .Include(x => x.IdAlbumNavigation).OrderBy(x => x.Nombre);
        }


    }


    public class AlbumRepository : Repository<Album>
    {
        public AlbumRepository(GroovehubContext context) : base(context)
        {
            
        }

        public Album? GetAlbum(int id)
        {
            return Context.Album
                .Include(x => x.Cancion).FirstOrDefault(x => x.Id == id);
        }
    }
}
