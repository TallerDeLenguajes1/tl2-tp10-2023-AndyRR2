using System.ComponentModel.DataAnnotations;

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class AsignarTareaViewModel{
        public int? Id{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id Usuario Asignado")]
        public int? IdUsuarioAsignado{get;set;}
        public List<int?> IdUsuarios{get;set;}

        public AsignarTareaViewModel(){
            IdUsuarios = new List<int?>();
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
            newTareaVM.IdUsuarioAsignado = newTarea.IdUsuarioAsignado;
            return(newTareaVM);
        }
        
    }
}