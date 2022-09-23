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
    public class ContratosController : Controller
    {
        private RepositorioContrato repoContrato;
        private RepositorioInmueble repoInmueble;
        private RepositorioInquilino repoInquilino;
        public ContratosController(IConfiguration configuration)
        {
            repoContrato = new RepositorioContrato(configuration);
            repoInmueble = new RepositorioInmueble(configuration);
            repoInquilino = new RepositorioInquilino(configuration);
        }
        // GET: Contratos
        [Authorize]
        public ActionResult Index()
        {
            var contrato = repoContrato.ObtenerContratos();
            return View(contrato);

        }

        // GET: Contratos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var contrato = repoContrato.ObtenerPorId(id);
            return View(contrato);
        }

        // GET: Contratos/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Inquilinos = repoInquilino.ObtenerInquilinos();
            ViewBag.Inmuebles = repoInmueble.ObtenerInmuebles();
            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                repoContrato.AltaContrato(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // GET: Contratos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            ViewBag.Inquilinos = repoInquilino.ObtenerInquilinos();
            ViewBag.Inmuebles = repoInmueble.ObtenerInmuebles();
            var contrato = repoContrato.ObtenerPorId(id);
            return View(contrato);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Contrato contrato)
        {
            Contrato c = null;
            try
            {
                c = repoContrato.ObtenerPorId(id);
                c.FechaDesde = contrato.FechaDesde;
                c.FechaHasta = contrato.FechaHasta;
                c.MontoAlquiler = contrato.MontoAlquiler;
                c.IdInmueble = contrato.IdInmueble;
                c.IdInquilino = contrato.IdInquilino;
                repoContrato.ModificacionContrato(c);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // GET: Contratos/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var contrato = repoContrato.ObtenerPorId(id);
            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Contrato contrato)
        {
            try
            {
                repoContrato.BajaContrato(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}