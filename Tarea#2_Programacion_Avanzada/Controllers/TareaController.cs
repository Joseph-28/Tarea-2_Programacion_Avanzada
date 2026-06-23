using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarea_2_Programacion_Avanzada.Models;

namespace Tarea_2_Programacion_Avanzada.Controllers
{
    public class TareaController : Controller
    {
        //Obtener lista desde la Sesión
        private List<Tarea> ObtenerTareasDeSesion()
        {
            if (Session["ListaTareas"] == null)
            {
                // Datos de prueba
                Session["ListaTareas"] = new List<Tarea>
                {
                    new Tarea { Id = 1, Titulo = "Grabar video de la tarea", Descripcion = "Video de 3 minutos explicando el proyecto", FechaLimite = DateTime.Today.AddDays(5), Prioridad = "Alta", Completada = false },
                    new Tarea { Id = 2, Titulo = "Tarea de prueba #2", Descripcion = "Esta es una tarea de prueba", FechaLimite = DateTime.Today.AddDays(4), Prioridad = "Media", Completada = false },
                    new Tarea { Id = 3, Titulo = "Tarea de prueba #3", Descripcion = "Esta es una tarea de prueba para comprobar el largo del texto en detalles", FechaLimite = DateTime.Today.AddDays(3), Prioridad = "Baja", Completada = false },
                };
            }
            return (List<Tarea>)Session["ListaTareas"];
        }

        //Vista Principal
        public ActionResult Index()
        {
            var tareas = ObtenerTareasDeSesion();

            //Carga de ViewBags
            ViewBag.TotalTareas = tareas.Count;
            ViewBag.Completadas = tareas.Count(t => t.Completada);

            return View(tareas);
        }
        //Vista Crear Tarea
        public ActionResult CrearTarea()
        {
            return View();
        }

        //Creacion de una tarea
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Tarea nuevaTarea)
        {
            if (ModelState.IsValid)
            {
                var tareas = ObtenerTareasDeSesion();

                //ID automatico
                nuevaTarea.Id = tareas.Count > 0 ? tareas.Max(t => t.Id) + 1 : 1;
                nuevaTarea.Completada = false;

                tareas.Add(nuevaTarea);
                Session["ListaTareas"] = tareas;

                // TempData para exito
                TempData["MensajeExito"] = "¡Tarea agregada correctamente!";
                return RedirectToAction("Index");
            }
            return View(nuevaTarea);
        }

        // Cambiar estado de la tarea
        [HttpPost]
        public JsonResult CambiarEstadoAjax(int id)
        {
            var tareas = ObtenerTareasDeSesion();
            var tarea = tareas.FirstOrDefault(t => t.Id == id);

            if (tarea != null)
            {
                // Invertir el estado
                tarea.Completada = !tarea.Completada;
                Session["ListaTareas"] = tareas;

                // Retornamos JSON
                return Json(new
                {
                    success = true,
                    nuevoEstado = tarea.Completada,
                    totalCompletadas = tareas.Count(t => t.Completada)
                });
            }

            return Json(new { success = false, message = "Tarea no encontrada." });
        }
        //Detalles
        [HttpGet]
        public ActionResult ObtenerDetalleAjax(int id)
        {
            var tareas = (List<Tarea>)Session["ListaTareas"];
            var tarea = tareas.FirstOrDefault(t => t.Id == id);

            if (tarea == null)
            {
                return HttpNotFound("La tarea solicitada no existe.");
            }

            return PartialView("_DetalleTarea", tarea);
        }
        //Eliminar tarea
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var tareas = (List<Tarea>)Session["ListaTareas"] ?? new List<Tarea>();
            var tareaAEliminar = tareas.FirstOrDefault(t => t.Id == id);

            if (tareaAEliminar != null)
            {
                tareas.Remove(tareaAEliminar);
                Session["ListaTareas"] = tareas;
            }

            return RedirectToAction("Index");
        }
        // Obtener datos para editar
        [HttpGet]
        public JsonResult ObtenerTareaPorIdAjax(int id)
        {
            var tareas = (List<Tarea>)Session["ListaTareas"] ?? new List<Tarea>();
            var tarea = tareas.FirstOrDefault(t => t.Id == id);

            if (tarea != null)
            {
                return Json(new
                {
                    success = true,
                    id = tarea.Id,
                    titulo = tarea.Titulo,
                    descripcion = tarea.Descripcion,
                    fecha = tarea.FechaLimite.ToString("yyyy-MM-dd"),
                    prioridad = tarea.Prioridad
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "Tarea no encontrada." }, JsonRequestBehavior.AllowGet);
        }

        // Editar datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Tarea tareaEditada)
        {
            var tareas = (List<Tarea>)Session["ListaTareas"] ?? new List<Tarea>();
            var tareaOriginal = tareas.FirstOrDefault(t => t.Id == tareaEditada.Id);

            if (tareaOriginal != null && ModelState.IsValid)
            {
                // Actualizar en memoria
                tareaOriginal.Titulo = tareaEditada.Titulo;
                tareaOriginal.Descripcion = tareaEditada.Descripcion;
                tareaOriginal.FechaLimite = tareaEditada.FechaLimite;
                tareaOriginal.Prioridad = tareaEditada.Prioridad;

                Session["ListaTareas"] = tareas;
                TempData["MensajeExito"] = "¡Tarea modificada correctamente!";
            }

            return RedirectToAction("Index");
        }
    }
}