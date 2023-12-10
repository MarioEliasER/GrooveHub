using GrooveHub.Repositories;
using GrooveHub.Areas.Admin.Models;
using GrooveHub.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrooveHub.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AlbumesController : Controller
    {
        public AlbumesController(Repository<Album> repository)
        {
            Repository = repository;
        }

        public Repository<Album> Repository { get; }

        [Authorize(Roles = "Supervisor, Administrador")]
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
        [Authorize(Roles = "Administrador")]
        public IActionResult Agregar()
        {

            AdminAgregarAlbumViewModel vm = new();
            return View(vm);
        }
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Supervisor, Administrador")]
        public IActionResult Editar(int id)
        {
            var album = Repository.Get(id);
            if (album == null)
            {
                return RedirectToAction("Index");
            }
            AdminAgregarAlbumViewModel vm = new()
            {
                Album = album
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor, Administrador")]
        public IActionResult Editar(AdminAgregarAlbumViewModel vm)
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
                    ModelState.AddModelError("", "Solamente se pueden subir imagenes en formato JPEG.");
                }
                if (vm.AlbumFile.Length > 5000 * 1024)
                {
                    ModelState.AddModelError("", "El máximo tamaño de archivo es de 5MB.");
                }
            }
            if (ModelState.IsValid)
            {
                var album = Repository.Get(vm.Album.Id);
                if (album == null)
                {
                    return RedirectToAction("Index");
                }
                album.TituloAlbum = vm.Album.TituloAlbum;
                album.FechaLanzamiento = vm.Album.FechaLanzamiento;
                Repository.Update(album);
                if (vm.AlbumFile != null)
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/albumes/{vm.Album.Id}.jpg");
                    vm.AlbumFile.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }
            return View(vm);
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            var album = Repository.Get(id);
            if (album == null)
            {
                return RedirectToAction("Index");
            }
            return View(album);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(Album a)
        {
            var album = Repository.Get(a.Id);
            if (album == null)
            {
                return RedirectToAction("Index");
            }
            Repository.Delete(album);
            var ruta = $"wwwroot/albumes/{a.Id}";
            if (System.IO.File.Exists(ruta))
            {
                System.IO.File.Delete(ruta);
            }
            return RedirectToAction("Index");
        }
    }
}
