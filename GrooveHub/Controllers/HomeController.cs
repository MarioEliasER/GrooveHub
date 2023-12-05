using FruitStore.Repositories;
using GrooveHub.Models.Entities;
using GrooveHub.Models.ViewModels;
using GrooveHub.Repositories;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace GrooveHub.Controllers
{
    public class HomeController : Controller
    {
        public Repository<Cancion> Cancionrepo { get; }
        public Repository<Album> Albumrepo { get; }
        public CancionesRepository Cancionesalbumrepo { get; }

        public HomeController(Repository<Cancion> cancionrepo, Repository<Album> albumrepo, CancionesRepository cancionesalbumrepo)
        {
            Cancionrepo = cancionrepo;
            Albumrepo = albumrepo;
            Cancionesalbumrepo = cancionesalbumrepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Biografia()
        {
            return View();
        }

        public IActionResult Discografia()
        {
            DiscografiaAlbumesViewModel vm = new()
            {
                Albumes = Albumrepo.GetAll().OrderBy(x => x.TituloAlbum).Select(x => new AlbumModel
                {
                    Id = x.Id,
                    Titulo = x.TituloAlbum,
                    FechaLanzamiento = x.FechaLanzamiento
                })
            };

            return View(vm);
        }

        public IActionResult Album(string Id)
        {
            Id = Id.Replace("-", " ");
            CancionesAlbumViewModel vm = new()
            {
                CancionesAlbumes = Cancionesalbumrepo.GetCancionesByAlbum(Id).Select(x => new CancionesAlbumModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Duracion = x.Duracion,
                    AñoLanzamiento = x.AñoLanzamiento,
                    IdAlbum = x.IdAlbumNavigation.Id
                })
            };
            return View(vm);
        }

        public IActionResult AparicionesEnTV()
        {
            return View();
        }
    }
}
