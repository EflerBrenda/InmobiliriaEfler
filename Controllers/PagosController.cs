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
    public class PagosController : Controller
    {
        private RepositorioPago repoPago;
        private RepositorioContrato repoContrato;
        public PagosController(IConfiguration configuration)
        {
            repoPago = new RepositorioPago(configuration);
            repoContrato = new RepositorioContrato(configuration);
        }
        // GET: Pagos
        public ActionResult Index()
        {
            var pagos = repoPago.ObtenerPagos();
            return View(pagos);
        }

        // GET: Pagos/Details/5
        public ActionResult Details(int id)
        {
            var pago = repoPago.ObtenerPorId(id);
            return View(pago);
        }

        // GET: Pagos/Create
        public ActionResult Create()
        {
            TempData["fechaActual"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
            ViewBag.Contratos = repoContrato.ObtenerContratos();
            return View();
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            try
            {
                repoPago.AltaPago(pago);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View("Views/Shared/ErrorInesperado.cshtml");
            }
        }

        // GET: Pagos/Edit/5
        public ActionResult Edit(int id)
        {
            TempData["Error"] = "";
            ViewBag.Contratos = repoContrato.ObtenerContratos();
            var pago = repoPago.ObtenerPorId(id);
            return View(pago);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago pago)
        {
            String mensaje = "";
            Pago p = repoPago.ObtenerPorId(id);
            try
            {
                if (pago.FechaPago == null || pago.FechaPago.Date.ToString("yyyy-MM-dd") == "0001-01-01")
                {
                    mensaje = "Debe ingresar una fecha valida de pago.";
                    return obtenerVistaEditar(mensaje, p);
                }
                if (pago.Descripcion == null || pago.Descripcion.Equals(""))
                {
                    mensaje = "Debe ingresar una descripción.";
                    return obtenerVistaEditar(mensaje, p);
                }
                if (pago.Importe == null || pago.Importe.Equals("") || pago.Importe.Equals("0"))
                {
                    mensaje = "Debe ingresar un importe valido.";
                    return obtenerVistaEditar(mensaje, p);
                }

                p.NumeroPago = p.NumeroPago;
                p.Descripcion = pago.Descripcion;
                p.FechaPago = pago.FechaPago;
                p.Importe = pago.Importe;
                p.IdContrato = p.IdContrato;
                repoPago.ModificacionPago(p);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View("Views/Shared/ErrorConstraint.cshtml");
            }
        }

        // GET: Pagos/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var pago = repoPago.ObtenerPorId(id);
            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repoPago.BajaPago(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View("Views/Shared/ErrorConstraint.cshtml");
            }
        }

        private ActionResult obtenerVistaEditar(String mensaje, Pago p)
        {
            TempData["Error"] = mensaje;
            ViewBag.Contratos = repoContrato.ObtenerContratos();
            return View(p);
        }
    }
}