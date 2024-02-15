using System.ComponentModel.DataAnnotations;

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class EditarTareaViewModel{
        [Display(Name = "Id")]
        public int? Id{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id Tablero")]
        public int? IdTablero{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre")]
        public string? Nombre{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Estado")]
        public EstadoTarea EstadoTarea{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Descripcion")]
        public string? Descripcion{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Color")]
        public Color Color{get;set;}
        public List<int?> IdTableros{get;set;}

        public EditarTareaViewModel(){
            IdTableros = new List<int?>();
        }
        public EditarTareaViewModel(int? id, int? idTablero, string? nombre, EstadoTarea estado, string? descripcion, Color color, List<int?> idTableros){
            Id=id;
            IdTablero=idTablero;
            Nombre=nombre;
            EstadoTarea=estado;
            Descripcion=descripcion;
            Color=color;
            IdTableros=idTableros;
        }

        public static EditarTareaViewModel FromTarea(Tarea newTarea)
        {
            EditarTareaViewModel newTareaVM = new EditarTareaViewModel();
            newTareaVM.Id = newTarea.Id;
            newTareaVM.IdTablero = newTarea.IdTablero;
            newTareaVM.Nombre = newTarea.Nombre;
            newTareaVM.EstadoTarea = (Proyecto.Models.EstadoTarea)newTarea.EstadoTarea;
            newTareaVM.Descripcion = newTarea.Descripcion;
            newTareaVM.Color = newTarea.Color;
            return(newTareaVM);
        }
        
    }
}