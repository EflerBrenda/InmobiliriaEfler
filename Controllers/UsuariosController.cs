using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaEfler.Models;

namespace InmobiliariaEfler.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositorioUsuario repoUsuario;
        private readonly IWebHostEnvironment environment;
        public UsuariosController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            repoUsuario = new RepositorioUsuario(configuration);
            this.environment = environment;
        }
        // GET: Usuarios
        public ActionResult Index()
        {
            var usuarios = repoUsuario.ObtenerUsuarios();
            return View(usuarios);
        }
        // GET Usuarios/Login
        public ActionResult Login()
        {

            return View();
        }


        // POST: Usuarios/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw;
            }
        }
        // GET: Usuarios/Details/5
        public ActionResult Details(int id)
        {
            var usuarios = repoUsuario.ObtenerPorId(id);
            return View(usuarios);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            try
            {
                if (usuario.AvatarFile != null)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "avatar" + Path.GetExtension(usuario.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usuario.Avatar = Path.Combine("/Uploads", fileName);
                    // Esta operación guarda la foto en memoria en la ruta que necesitamos
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        usuario.AvatarFile.CopyTo(stream);
                    }

                }
                repoUsuario.AltaUsuario(usuario);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
            var usuario = repoUsuario.ObtenerPorId(id);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Usuario usuario)
        {
            Usuario u = null;
            try
            {
                u = repoUsuario.ObtenerPorId(id);
                u.Nombre = usuario.Nombre;
                u.Apellido = usuario.Apellido;
                u.Email = usuario.Email;
                u.Password = usuario.Password;
                u.Avatar = usuario.Avatar;
                u.Rol = usuario.Rol;
                repoUsuario.ModificacionUsuario(u);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int id)
        {
            var usuario = repoUsuario.ObtenerPorId(id);
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {

                repoUsuario.BajaUsuario(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw;
            }
        }




    }
}