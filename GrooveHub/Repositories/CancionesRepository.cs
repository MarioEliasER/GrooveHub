using FruitStore.Repositories;
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

}
