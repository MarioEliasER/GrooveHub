using FruitStore.Repositories;
using GrooveHub.Areas.Admin.Models;
using GrooveHub.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GrooveHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AlbumesController : Controller
    {
        public AlbumesController(Repository<Album> repository)
        {
            Repository = repository;
        }

        public Repository<Album> Repository { get; }

        public IActionResult Index()
        {
            AdminAlbumesViewModel vm = new()
            {
                Albumes = Repository.GetAll().OrderBy(x => x.TituloAlbum).Select(x => new AdminAlbumModel
                {
                    Id = x.Id,
                    NombreAlbum = x.TituloAlbum
                })
            };
            return View(vm);
        }

        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Agregar(AdminAgregarAlbumViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Album.TituloAlbum))
            {
                ModelState.AddModelError("", "El titulo no puede estar vacío.");
            }
            if ((vm.Album.FechaLanzamiento.Year < 1990 || vm.Album.FechaLanzamiento.Year > DateTime.Now.Year) ||
            (vm.Album.FechaLanzamiento.Year == DateTime.Now.Year &&
            (vm.Album.FechaLanzamiento.Month > DateTime.Now.Month ||
            (vm.Album.FechaLanzamiento.Month == DateTime.Now.Month && vm.Album.FechaLanzamiento.Day > DateTime.Now.Day))))
            {
                ModelState.AddModelError("", "La fecha es incorrecta");
            }
            if (vm.AlbumFile != null)
            {
                if (vm.AlbumFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "Solo se permiten imagenes JPEG.");
                }
                if (vm.AlbumFile.Length > 5000 * 1024)
                {
                    ModelState.AddModelError("", "Solo se permiten archivos no mayores a 5MB.");
                }
            }
            if (ModelState.IsValid)
            {
                Repository.Insert(vm.Album);
                if (vm.AlbumFile == null)
                {
                    System.IO.File.Copy("wwwroot/images/no-disponbile.jpg", $"wwwroot/albumes/{vm.Album.Id}.jpg");
                }
                else
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/albumes/{vm.Album.Id}.jpg");
                    vm.AlbumFile.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }
            return View(vm);
        }
    }
}
