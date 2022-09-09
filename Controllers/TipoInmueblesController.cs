using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaEfler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaEfler.Controllers
{
    public class TipoInmueblesController : Controller
    {
        private RepositorioTipoInmueble repo;
        public TipoInmueblesController(IConfiguration configuration)
        {
            repo = new RepositorioTipoInmueble(configuration);
        }
        // GET: TipoInmuebles
        public ActionResult Index()
        {
            /* var tipoInmuebles = repo.ObtenertipoInmuebles();
            return View(tipoInmuebles);*/
            return View();
        }

        // GET: TipoInmuebles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TipoInmuebles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoInmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TipoInmuebles/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TipoInmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TipoInmuebles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TipoInmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}