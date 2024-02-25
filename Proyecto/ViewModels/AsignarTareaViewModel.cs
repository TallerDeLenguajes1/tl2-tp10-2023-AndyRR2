using System.ComponentModel.DataAnnotations;//Necesario para uso de Atributos

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class AsignarTareaViewModel{
        public int? Id{get;set;}//No necesita atributos ya que va oculto en el View

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id Usuario Asignado")]
        public int? IdUsuarioAsignado{get;set;}
        
        public List<int?> IdUsuarios{get;set;}//Necesario para guardar la lista de Id seleccionables obtenidos de la DB
        public AsignarTareaViewModel(){
            IdUsuarios = new List<int?>();//Asegura que siempre tenga una instancia de la lista válida
        }
        public AsignarTareaViewModel(int? id, int? idUsuarioAsig, List<int?> idUsuarios){
            Id=id;
            IdUsuarioAsignado=idUsuarioAsig;
            IdUsuarios=idUsuarios;
        }
        public static AsignarTareaViewModel FromTarea(Tarea newTarea)
        {
            AsignarTareaViewModel newTareaVM = new AsignarTareaViewModel();
            newTareaVM.Id = newTarea.Id;
            return(newTareaVM);
        }
    }
}