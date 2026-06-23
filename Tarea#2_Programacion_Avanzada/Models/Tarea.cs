using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tarea_2_Programacion_Avanzada.Models
{
    public class Tarea
    {
        //ID
        public int Id { get; set; }

        //Titulo
        [Required(ErrorMessage = "El título de la tarea es obligatorio.")]
        [StringLength(50, ErrorMessage = "El título no puede exceder los 50 caracteres.")]
        [Display(Name = "Título de la Tarea")]
        public string Titulo { get; set; }

        //Descripcion
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(150, ErrorMessage = "La descripción no puede superar los 150 caracteres.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        //Fecha
        [Required(ErrorMessage = "La fecha límite es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Límite")]
        public DateTime FechaLimite { get; set; }

        //Prioridad
        [Display(Name = "Prioridad")]
        public string Prioridad { get; set; }

        //Completada
        [Display(Name = "Completada")]
        public bool Completada { get; set; }

    }
}