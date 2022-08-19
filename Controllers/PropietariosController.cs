using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaEfler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaEfler.Controllers
{
    public class PropietariosController : Controller
    {
        RepositorioPropietario repo;
        public PropietariosController()
        {
            repo = new RepositorioPropietario();
        }
        // GET: Propietarios
        public ActionResult Index()
        {
            var propietarios = repo.ObtenerPropietarios();
            return View(propietarios);
        }

        // GET: Propietarios/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Propietarios/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                repo.AltaPropietario(propietario);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Edit/5y
        public ActionResult Edit(int id)
        {
            var propietario = repo.ObtenerPorId(id);
            return View(propietario);
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {

                repo.ModificacionPropietario(id, propietario);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario propietario)
        {
            try
            {

                repo.BajaPropietario(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}