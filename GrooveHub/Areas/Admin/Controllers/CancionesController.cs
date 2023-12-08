using FruitStore.Repositories;
using GrooveHub.Areas.Admin.Models;
using GrooveHub.Models.Entities;
using GrooveHub.Repositories;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Linq;
using System.Text.RegularExpressions;

namespace GrooveHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CancionesController : Controller
    {
        public CancionesRepository Repository { get;}

        //public Repository<Cancion> Repository { get; }

        public Repository<Album> Albumrepository { get; }

        //public CancionesController(Repository<Cancion> repository, Repository<Album> albumrepository)
        //{
        //    Repository = repository;
        //    Albumrepository = albumrepository;
        //}

        public CancionesController(CancionesRepository repository, Repository<Album> albumrepository)
        {
            Repository = repository;
            Albumrepository = albumrepository;
        }

        public IActionResult Index()
        {
            AdminCancionesViewModel vm = new()
            {
                Canciones = Repository.GetAll().OrderBy(x => x.Nombre).Select(x => new AdminCancionesModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    AñoLanzamiento = x.AñoLanzamiento,
                    Duracion = x.Duracion,
                    IdAlbum = x.IdAlbumNavigation.Id,
                    NombreAlbum = x.IdAlbumNavigation.TituloAlbum
                })
            };
            return View(vm);
        }

        public IActionResult Agregar()
        {
            AdminAgregarCancionViewModel vm = new();
            vm.Albumes = Albumrepository.GetAll().OrderBy(x => x.TituloAlbum).Select(x => new AdminAlbumModel
            {
                Id = x.Id,
                NombreAlbum = x.TituloAlbum
            });
            return View(vm);
        }

        [HttpPost]
        public IActionResult Agregar(AdminAgregarCancionViewModel vm)
        {
            ModelState.Clear();

            if (string.IsNullOrWhiteSpace(vm.Cancion.Nombre))
            {
                ModelState.AddModelError("", "El nombre de la canción no puede estar vacío.");
            }
            if (vm.Cancion.AñoLanzamiento.Year < 1990 || vm.Cancion.AñoLanzamiento.Year > DateTime.Now.Year)
            {
                ModelState.AddModelError("", "La fecha tiene un formato incorrecto.");
            }
            if (vm.Cancion.Duracion.Hour == 0)
            {
                vm.Cancion.Duracion = new TimeOnly(0, vm.Cancion.Duracion.Minute, vm.Cancion.Duracion.Second);
            }

            //var albumes = Albumrepository.GetAll();
            //List<int> albumesids = albumes.Select(x => x.Id).ToList();
            vm.Albumes = Albumrepository.GetAll().Select(x => new AdminAlbumModel
            {
                Id = x.Id,
                NombreAlbum = x.TituloAlbum
            });
            List<int> albumesids = vm.Albumes.Select(x => x.Id).ToList();

            if (!albumesids.Contains(vm.Cancion.IdAlbum))
            {
                ModelState.AddModelError("", "Selecciona un album válido.");
            }
            if (ModelState.IsValid)
            {
                Repository.Insert(vm.Cancion);
                //System.IO.FileStream fs = System.IO.File.Create($"wwwroot/canciones/{vm.Cancion.IdAlbum}.jpg");
                System.IO.File.Copy($"wwwroot/albumes/{vm.Cancion.IdAlbum}.jpg", $"wwwroot/canciones/{vm.Cancion.Id}.jpg");
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        public IActionResult Editar(int id)
        {
            var cancion = Repository.Get(id);
            if (cancion == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AdminAgregarCancionViewModel vm = new();
                vm.Cancion = cancion;
                vm.Albumes = Albumrepository.GetAll().OrderBy(x => x.TituloAlbum).Select(x => new AdminAlbumModel
                {
                    Id = x.Id,
                    NombreAlbum = x.TituloAlbum
                });
                return View(vm);
            }
        }

        [HttpPost]
        public IActionResult Editar(AdminAgregarCancionViewModel vm)
        {
            ModelState.Clear();

            if (string.IsNullOrWhiteSpace(vm.Cancion.Nombre))
            {
                ModelState.AddModelError("", "El nombre de la canción no puede estar vacío.");
            }
            if ((vm.Cancion.AñoLanzamiento.Year < 1990 || vm.Cancion.AñoLanzamiento.Year > DateTime.Now.Year) ||
                        (vm.Cancion.AñoLanzamiento.Year == DateTime.Now.Year &&
                        (vm.Cancion.AñoLanzamiento.Month > DateTime.Now.Month ||
                        (vm.Cancion.AñoLanzamiento.Month == DateTime.Now.Month && vm.Cancion.AñoLanzamiento.Day > DateTime.Now.Day))))
            {
                ModelState.AddModelError("", "La fecha tiene un formato incorrecto.");
            }
            if (vm.Cancion.Duracion.Hour == 0)
            {
                vm.Cancion.Duracion = new TimeOnly(0, vm.Cancion.Duracion.Minute, vm.Cancion.Duracion.Second);
            }
            //var albumes = Albumrepository.GetAll();
            //List<int> albumesids = albumes.Select(x => x.Id).ToList();
            //if (!albumesids.Contains(vm.Cancion.IdAlbum))
            //{
            //    ModelState.AddModelError("", "Selecciona un album válido.");
            //}

            vm.Albumes = Albumrepository.GetAll().Select(x => new AdminAlbumModel
            {
                Id = x.Id,
                NombreAlbum = x.TituloAlbum
            });
            List<int> albumesids = vm.Albumes.Select(x => x.Id).ToList();
            if (ModelState.IsValid)
            {
                var cancion = Repository.Get(vm.Cancion.Id);
                if (cancion == null)
                {
                    return RedirectToAction("Index");
                }
                cancion.AñoLanzamiento = vm.Cancion.AñoLanzamiento;
                cancion.Duracion = vm.Cancion.Duracion;
                cancion.IdAlbum = vm.Cancion.IdAlbum;
                cancion.Nombre = vm.Cancion.Nombre;
                Repository.Update(cancion);
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        public IActionResult Eliminar(int id)
        {
            var cancion = Repository.Get(id);
            if (cancion == null)
            {
                return RedirectToAction("Index");
            }
            return View(cancion);
        }

        [HttpPost]
        public IActionResult Eliminar(Cancion c)
        {
            var cancion = Repository.Get(c.Id);
            if (cancion == null)
            {
                return RedirectToAction("Index");
            }
            Repository.Delete(cancion);
            return RedirectToAction("Index");
        }
    }
}
