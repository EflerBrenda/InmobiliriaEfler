using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaEfler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaEfler.Controllers
{
    public class InmueblesController : Controller
    {
        private RepositorioInmueble repoInmueble;
        private RepositorioPropietario repoPropietario;
        private RepositorioTipoInmueble repoTipoInmueble;
        public InmueblesController()
        {
            repoInmueble = new RepositorioInmueble();
            repoPropietario = new RepositorioPropietario();
            repoTipoInmueble = new RepositorioTipoInmueble();
        }
        // GET: Inmuebles
        public ActionResult Index()
        {
            var inmuebles = repoInmueble.ObtenerInmuebles();
            return View(inmuebles);
        }

        // GET: Inmuebles/Details/5
        public ActionResult Details(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            return View(inmueble);
        }

        // GET: Inmuebles/Create
        public ActionResult Create()
        {
            ViewBag.Propietarios = repoPropietario.ObtenerPropietarios();
            ViewBag.TipoInmueble = repoTipoInmueble.ObtenerTipoInmueble();
            return View();
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                // TODO: Add insert logic here
                repoInmueble.AltaInmueble(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inmuebles/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Propietarios = repoPropietario.ObtenerPropietarios();
            ViewBag.TipoInmueble = repoTipoInmueble.ObtenerTipoInmueble();
            var inmueble = repoInmueble.ObtenerPorId(id);
            return View(inmueble);
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            Inmueble i = null;

            try
            {
                i = repoInmueble.ObtenerPorId(id);
                i.Direccion = inmueble.Direccion;
                i.Ambientes = inmueble.Ambientes;
                i.Superficie = inmueble.Superficie;
                i.Latitud = inmueble.Latitud;
                i.Longitud = inmueble.Longitud;
                i.Precio = inmueble.Precio;
                i.Coordenadas = inmueble.Coordenadas;
                i.OfertaActiva = inmueble.OfertaActiva;
                i.IdPropietario = inmueble.IdPropietario;
                i.IdTipo = inmueble.IdTipo;
                repoInmueble.ModificacionInmueble(i);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inmuebles/Delete/5
        public ActionResult Delete(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            return View(inmueble);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repoInmueble.BajaInmueble(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}