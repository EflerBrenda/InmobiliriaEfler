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
            String mensaje = "";
            return obtenerVista(mensaje);
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                String mensaje = "";
                if (contrato.IdInmueble == null || contrato.IdInmueble < 0)
                {
                    mensaje = "Debe ingresar un inmueble.";
                    return obtenerVista(mensaje);
                }
                if (contrato.FechaDesde == null || contrato.FechaDesde.Date.ToString("yyyy-MM-dd") == "0001-01-01")
                {
                    mensaje = "Debe ingresar una fecha valida desde.";
                    return obtenerVista(mensaje);
                }
                if (contrato.FechaHasta == null || contrato.FechaHasta.Date.ToString("yyyy-MM-dd") == "0001-01-01")
                {
                    mensaje = "Debe ingresar una fecha valida hasta.";
                    return obtenerVista(mensaje);
                }
                if (contrato.FechaDesde > contrato.FechaHasta)
                {
                    mensaje = "La fecha inicio no puede ser mayor que la fecha de fin.";
                    return obtenerVista(mensaje);
                }
                Contrato c = repoContrato.ComprobarDisponibilidad(contrato);
                if (c != null)
                {
                    mensaje = "Inmueble no disponible en esas fechas";
                    return obtenerVista(mensaje);
                }
                repoContrato.AltaContrato(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                String error = "Error inesperado, por favor vuelva a intentarlo";
                return obtenerVista(error);
            }
        }
        // GET: Contratos/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            TempData["Error"] = "";
            var contrato = repoContrato.ObtenerPorId(id);
            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Contrato contrato)
        {
            Contrato c = repoContrato.ObtenerPorId(id);
            try
            {
                Contrato cv = repoContrato.ObtenerContratoVigente(id);
                if (cv != null)
                {
                    TempData["Error"] = "No se puede eliminar el contrato ya que esta vigente.";
                    return View(c);
                }
                else
                {
                    repoContrato.BajaContrato(id);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error inesperado, por favor vuelva a intentarlo";
                return View(c);
            }
        }

        [Authorize]
        public ActionResult VerContratosVigentes()
        {
            var contrato = repoContrato.ObtenerContratosVigentes();
            return View(contrato);

        }
        [Authorize]
        public ActionResult VerContratosNoVigentes()
        {
            var contrato = repoContrato.ObtenerContratosNoVigentes();
            return View(contrato);

        }
        [Authorize]
        public ActionResult RenovarContrato(int id)
        {
            TempData["Error"] = "";
            var contrato = repoContrato.ObtenerPorId(id);
            ViewBag.Inquilinos = repoInquilino.ObtenerInquilinos();
            ViewBag.Inmuebles = repoInmueble.ObtenerInmuebles();
            return View(contrato);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult RenovarContrato(int id, Contrato con)
        {
            Contrato contrato = repoContrato.ObtenerPorId(id);
            try
            {

                String mensaje = "";
                if (con.FechaDesde == null || con.FechaDesde.Date.ToString("yyyy-MM-dd") == "0001-01-01")
                {
                    mensaje = "Debe ingresar una fecha valida desde.";
                    return obtenerVistaEditar(mensaje, contrato);
                }
                if (con.FechaHasta == null || con.FechaHasta.Date.ToString("yyyy-MM-dd") == "0001-01-01")
                {
                    mensaje = "Debe ingresar una fecha valida hasta.";
                    return obtenerVistaEditar(mensaje, contrato);
                }
                if (con.FechaHasta < con.FechaDesde)
                {
                    mensaje = "La fecha hasta no puede ser menor que la fecha desde.";
                    return obtenerVistaEditar(mensaje, contrato);
                }

                Contrato c = repoContrato.ComprobarDisponibilidad(con);
                if (c != null)
                {
                    mensaje = "Inmueble no disponible en esas fechas";
                    return obtenerVistaEditar(mensaje, contrato);
                }
                Contrato contratoNuevo = new Contrato();
                contratoNuevo.Id = contrato.Id;
                contratoNuevo.FechaDesde = con.FechaDesde;
                contratoNuevo.FechaHasta = con.FechaHasta;
                contratoNuevo.IdInmueble = contrato.IdInmueble;
                contratoNuevo.IdInquilino = contrato.IdInquilino;
                contratoNuevo.MontoAlquiler = con.MontoAlquiler;
                repoContrato.ModificacionContrato(contratoNuevo);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception e)
            {
                String error = "Error inesperado, por favor vuelva a intentarlo";
                return obtenerVistaEditar(error, contrato);
            }
        }

        [Authorize]
        public ActionResult VerPagos(int id)
        {
            var pagos = repoContrato.ObtenerPagosPorContrato(id);
            return View(pagos);

        }

        [Authorize]
        public ActionResult FiltrarDisponibles()
        {
            TempData["Error"] = "";
            TempData["FechaDesde"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
            TempData["FechaHasta"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult FiltrarDisponibles(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                TempData["FechaDesde"] = fechaDesde.Date.ToString("yyyy-MM-dd");
                TempData["FechaHasta"] = fechaHasta.Date.ToString("yyyy-MM-dd");
                if (fechaDesde == null || fechaDesde.Date.ToString("yyyy-MM-dd") == "0001-01-01")
                {
                    TempData["Error"] = "Ingrese fecha desde valida.";
                    return View();

                }
                if (fechaHasta == null || fechaHasta.Date.ToString("yyyy-MM-dd") == "0001-01-01")
                {

                    TempData["Error"] = "Ingrese fecha hasta valida.";
                    return View();
                }
                if (fechaHasta < fechaDesde)
                {
                    TempData["Error"] = "La fecha hasta no debe ser menor a la fecha desde.";
                    return View();
                }
                var listaInmuebles = repoContrato.ObtenerInmueblesDisponibles(fechaDesde, fechaHasta);
                TempData["Error"] = "";
                return View(listaInmuebles);
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error inesperado, por favor vuelva a intentarlo";

                return View();
            }
        }

        private ActionResult obtenerVista(String mensaje)
        {
            TempData["Error"] = mensaje;
            ViewBag.Inquilinos = repoInquilino.ObtenerInquilinos();
            ViewBag.Inmuebles = repoInmueble.ObtenerInmuebles();
            return View();
        }
        private ActionResult obtenerVistaEditar(String mensaje, Contrato c)
        {
            TempData["Error"] = mensaje;
            ViewBag.Inquilinos = repoInquilino.ObtenerInquilinos();
            ViewBag.Inmuebles = repoInmueble.ObtenerInmuebles();
            return View(c);
        }

    }
}