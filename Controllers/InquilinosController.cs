using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaEfler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaEfler.Controllers
{
    public class InquilinosController : Controller
    {
        private RepositorioInquilino repo;
        private RepositorioContrato repoContrato;
        public InquilinosController(IConfiguration configuration)
        {
            repo = new RepositorioInquilino(configuration);
            repoContrato = new RepositorioContrato(configuration);
        }
        // GET: inquilinos
        [Authorize]
        public ActionResult Index()
        {
            var inquilinos = repo.ObtenerInquilinos();
            return View(inquilinos);
        }

        // GET: inquilinos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var inquilino = repo.ObtenerPorId(id);
            return View(inquilino);
        }

        // GET: inquilinos/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize]
        // POST: inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                repo.AltaInquilino(inquilino);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View("Views/Shared/ErrorInesperado.cshtml");
            }
        }

        // GET: inquilinos/Edit/5y
        [Authorize]
        public ActionResult Edit(int id)
        {
            var inquilino = repo.ObtenerPorId(id);
            return View(inquilino);
        }

        // POST: inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inquilino inquilino)
        {
            Inquilino p = null;
            try
            {
                p = repo.ObtenerPorId(id);
                p.Nombre = inquilino.Nombre;
                p.Apellido = inquilino.Apellido;
                p.DNI = inquilino.DNI;
                p.Telefono = inquilino.Telefono;
                p.Email = inquilino.Email;
                repo.ModificacionInquilino(p);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View("Views/Shared/ErrorConstraint.cshtml");
            }
        }

        // GET: inquilinos/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            TempData["error"] = "";
            var inquilino = repo.ObtenerPorId(id);
            return View(inquilino);
        }

        // POST: inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inquilino inquilino)
        {
            try
            {
                Inquilino i = repo.ObtenerPorId(id);
                List<Contrato> listaContratos = repoContrato.ObtenerContratosPorInquilino(id);
                if (listaContratos.Count != 0)
                {
                    TempData["error"] = "No se puede eliminar el inquilino ya que esta asociado a un contrato.";
                    return View(i);
                }
                repo.BajaInquilino(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View("Views/Shared/ErrorConstraint.cshtml");
            }
        }
    }
}