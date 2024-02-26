using System.ComponentModel.DataAnnotations;//Necesario para uso de Atributos

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class CambiarEstadoTareaViewModel{
        public int? Id{get;set;}//No necesita atributos ya que va oculto en el View

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Estado")]
        public EstadoTarea EstadoTarea{get;set;}
        
        public CambiarEstadoTareaViewModel(){}
        public CambiarEstadoTareaViewModel(int? id, EstadoTarea estado){
            Id=id;
            EstadoTarea = estado;
        }
        public static CambiarEstadoTareaViewModel FromTarea(Tarea newTarea)
        {
            CambiarEstadoTareaViewModel newTareaVM = new CambiarEstadoTareaViewModel();
            newTareaVM.Id = newTarea.Id;
            newTareaVM.EstadoTarea = newTarea.EstadoTarea;
            return(newTareaVM);
        }
    }
}