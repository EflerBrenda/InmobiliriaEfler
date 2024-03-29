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
    public class InmueblesController : Controller
    {
        private RepositorioInmueble repoInmueble;
        private RepositorioPropietario repoPropietario;
        private RepositorioTipoInmueble repoTipoInmueble;
        private RepositorioContrato repoContrato;

        //private RepositorioBase repoBase=new RepositorioBase();
        public InmueblesController(IConfiguration configuration)
        {
            repoInmueble = new RepositorioInmueble(configuration);
            repoPropietario = new RepositorioPropietario(configuration);
            repoTipoInmueble = new RepositorioTipoInmueble(configuration);
            repoContrato = new RepositorioContrato(configuration);

        }
        // GET: Inmuebles
        [Authorize]
        public ActionResult Index()
        {
            var inmuebles = repoInmueble.ObtenerInmuebles();
            return View(inmuebles);
        }
        [Authorize]
        // GET: Inmuebles/Details/5
        public ActionResult Details(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            return View(inmueble);
        }

        // GET: Inmuebles/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Propietarios = repoPropietario.ObtenerPropietarios();
            ViewBag.TipoInmueble = repoTipoInmueble.ObtenerTipoInmueble();
            ViewBag.Usos = Inmueble.ObtenerUsos();
            return View();
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                // TODO: Add insert logic here
                repoInmueble.AltaInmueble(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View("Views/Shared/ErrorInesperado.cshtml");
            }
        }

        // GET: Inmuebles/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            ViewBag.Propietarios = repoPropietario.ObtenerPropietarios();
            ViewBag.TipoInmueble = repoTipoInmueble.ObtenerTipoInmueble();
            ViewBag.Usos = Inmueble.ObtenerUsos();
            var inmueble = repoInmueble.ObtenerPorId(id);
            return View(inmueble);
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            Inmueble i = null;

            try
            {
                i = repoInmueble.ObtenerPorId(id);
                i.Direccion = inmueble.Direccion;
                i.Ambientes = inmueble.Ambientes;
                i.Latitud = inmueble.Latitud;
                i.Longitud = inmueble.Longitud;
                i.Precio = inmueble.Precio;
                i.OfertaActiva = inmueble.OfertaActiva;
                i.IdPropietario = inmueble.IdPropietario;
                i.Uso = inmueble.Uso;
                i.IdTipo = inmueble.IdTipo;
                repoInmueble.ModificacionInmueble(i);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View("Views/Shared/ErrorConstraint.cshtml");
            }
        }

        // GET: Inmuebles/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            TempData["error"] = "";
            var inmueble = repoInmueble.ObtenerPorId(id);
            return View(inmueble);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inmueble inmueble)
        {
            try
            {
                Inmueble i = repoInmueble.ObtenerPorId(id);
                List<Contrato> listaContratos = repoContrato.ObtenerContratosPorInmueble(id);
                if (listaContratos.Count != 0)
                {
                    TempData["error"] = "No se puede eliminar el inmueble ya que esta asociado a un contrato.";
                    return View(i);
                }
                repoInmueble.BajaInmueble(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View("Views/Shared/ErrorConstraint.cshtml");
            }
        }
        [Authorize]
        public ActionResult VerDisponibles()
        {
            var inmuebles = repoInmueble.ObtenerDisponibles();
            return View(inmuebles);
        }
        [Authorize]
        public ActionResult VerContratos(int id)
        {
            var inmuebles = repoInmueble.ObtenerContratosPorInmueble(id);
            return View(inmuebles);
        }

    }
}