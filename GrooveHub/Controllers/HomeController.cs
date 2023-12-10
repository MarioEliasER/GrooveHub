using GrooveHub.Models.Entities;
using GrooveHub.Models.ViewModels;
using GrooveHub.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;
using GrooveHub.Helpers;

namespace GrooveHub.Controllers
{
    public class HomeController : Controller
    {
        public Repository<Cancion> Cancionrepo { get; }
        public Repository<Album> Albumrepo { get; }
        public CancionesRepository Cancionesalbumrepo { get; }
        public Repository<Usuario> Userrepo { get; }

        public HomeController(Repository<Cancion> cancionrepo, Repository<Album> albumrepo, CancionesRepository cancionesalbumrepo, Repository<Usuario> userrepo)
        {
            Cancionrepo = cancionrepo;
            Albumrepo = albumrepo;
            Cancionesalbumrepo = cancionesalbumrepo;
            Userrepo = userrepo;
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
                Albumes = Albumrepo.GetAll().OrderBy(x => x.Id).Select(x => new AlbumModel
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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Correo))
            {
                ModelState.AddModelError("", "El correo no puede estar vacío.");
            }
            if (string.IsNullOrWhiteSpace(vm.Contrasena))
            {
                ModelState.AddModelError("", "La contraseña no puede estar vacía.");
            }
            if (ModelState.IsValid)
            {
                var user = Userrepo.GetAll().FirstOrDefault(x => x.CorreoElectronico == vm.Correo && x.Contrasena == Encriptacion.StringToSHA512(vm.Contrasena));
                if (user == null)
                {
                    ModelState.AddModelError("", "Contraseña o Correo Electrónico incorrectos.");
                }
                else
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("Id", user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.Nombre));
                    claims.Add(new Claim(ClaimTypes.Role, user.Rol == 1 ? "Administrador" : "Supervisor"));

                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(new ClaimsPrincipal(identity));
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            return View(vm);
        }

        public IActionResult Denied()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
