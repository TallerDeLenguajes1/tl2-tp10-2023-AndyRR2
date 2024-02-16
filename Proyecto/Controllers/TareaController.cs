using Microsoft.AspNetCore.Mvc;//Necesario para eredar el Controller proporcionado

using Proyecto.Repositories;
using Proyecto.Models;
using Proyecto.ViewModels;

namespace Proyecto.Controllers{
    public class TareaController: Controller{
        private readonly string direccionBD;
        private readonly ITareaRepository repoTarea;
        private readonly IUsuarioRepository repoUsuario;
        private readonly ITableroRepository repoTablero;
        private readonly ILoginRepository repoLogin;
        private readonly ILogger<HomeController> _logger;
        public TareaController(ILogger<HomeController> logger, ITareaRepository tareRepo, string cadenaDeConexion, IUsuarioRepository usuRepo, ITableroRepository tabRepo, ILoginRepository logRepo) 
        {
            _logger = logger;
            repoTarea = tareRepo;
            repoUsuario = usuRepo;
            repoTablero = tabRepo;
            repoLogin = logRepo;
            direccionBD = cadenaDeConexion;
        }

        public IActionResult Index(int? idTablero){
            try
            {
                if(!isLogin()) return RedirectToAction("Index","Login"); 

                List<Tarea> tareas = new List<Tarea>();

                if(isAdmin()){
                    if (idTablero.HasValue){
                        tareas = repoTarea.GetByOwnerBoard(idTablero);
                    }else{
                        tareas = repoTarea.GetAll();
                    }
                }else if(idTablero.HasValue){
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if ((repoTablero.GetById(idTablero).IdUsuarioPropietario == usuarioLogeado.Id) || repoTarea.ChechAsignedTask(idTablero,usuarioLogeado.Id)){
                        tareas = repoTarea.GetByOwnerBoard(idTablero);   
                    }else{
                        return NotFound();
                    }
                }else{
                    return NotFound();
                }

                List<ListarTareaViewModel> listaTareasVM = ListarTareaViewModel.FromTarea(tareas);
                return View(listaTareasVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult AgregarTarea(){
            try
            {
                if(!isLogin()) return RedirectToAction("Index","Login");
                if(!isAdmin()) return NotFound();

                CrearTareaViewModel newTareaVM = new CrearTareaViewModel();
                newTareaVM.IdUsuarioAsignado = 0;

                List<Usuario> usuariosEnBD = repoUsuario.GetAll();
                foreach (var usuario in usuariosEnBD)
                {
                    (newTareaVM.IdUsuarios).Add(usuario.Id);
                }

                List<Tablero> tablerosEnBD = repoTablero.GetAll();
                foreach (var tablero in tablerosEnBD)
                {
                    (newTareaVM.IdTableros).Add(tablero.Id);
                }
                return View(newTareaVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult AgregarTareaFromForm(CrearTareaViewModel newTareaVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin()) return RedirectToAction("Index","Login");
                if(!isAdmin()) return NotFound();

                Tarea newTarea = Tarea.FromCrearTareaViewModel(newTareaVM);
                repoTarea.Create(newTarea);
                return RedirectToAction("Index", new { iTablero = newTarea.IdTablero });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult EditarTarea(int? idTarea){  
            try
            {
                if(!isLogin()) return RedirectToAction("Index","Login");

                Tarea tareaAEditar = repoTarea.GetById(idTarea);
                EditarTareaViewModel tareaAEditarVM = new EditarTareaViewModel();
                
                if (isAdmin()){
                    tareaAEditarVM = EditarTareaViewModel.FromTarea(tareaAEditar);
                }else if (idTarea.HasValue){
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == repoTarea.GetById(idTarea).IdUsuarioPropietario){
                        tareaAEditarVM = EditarTareaViewModel.FromTarea(tareaAEditar);
                    }else{
                        return NotFound();
                    }
                }else{
                    return NotFound();
                }

                List<Tablero> tablerosEnBD = repoTablero.GetAll();
                foreach (var tablero in tablerosEnBD)
                {
                    (tareaAEditarVM.IdTableros).Add(tablero.Id);
                }
                return View(tareaAEditarVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult EditarTareaFromForm(EditarTareaViewModel tareaAEditarVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin()) return RedirectToAction("Index","Login"); 

                Tarea tareaAEditar = Tarea.FromEditarTareaViewModel(tareaAEditarVM);
                repoTarea.Update(tareaAEditar);
                return RedirectToAction("Index", new { idTablero = tareaAEditar.IdTablero });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            } 
        }

        [HttpGet]
        public IActionResult EliminarTarea(int? idTarea){
            try
            {
                if(!isLogin()) return RedirectToAction("Index","Login");

                Tarea tareaAEliminar = repoTarea.GetById(idTarea);

                if (isAdmin()){
                    return View(tareaAEliminar);
                }else if(idTarea.HasValue){
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == repoTarea.GetById(idTarea).IdUsuarioPropietario){
                        return View(tareaAEliminar);
                    }else{
                        return NotFound();
                    }
                }else{
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            } 
        }
        [HttpPost]
        public IActionResult EliminarFromForm(Tarea tareaAEliminar){
            try
            {
                if(!isLogin()) return RedirectToAction("Index","Login");

                repoTarea.Remove(tareaAEliminar.Id);
                return RedirectToAction("Index", "Usuario");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult AsignarTarea(int? idTarea){
            try
            {
                if(!isLogin()) return RedirectToAction("Index","Login");
                Tarea tareaSelec = repoTarea.GetById(idTarea);
                AsignarTareaViewModel tareaSelecVM = new AsignarTareaViewModel();
                if (isAdmin()){
                    tareaSelecVM = AsignarTareaViewModel.FromTarea(tareaSelec);
                }else if (idTarea.HasValue){
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == repoTarea.GetById(idTarea).IdUsuarioPropietario){
                        tareaSelecVM = AsignarTareaViewModel.FromTarea(tareaSelec);
                    }else{
                        return NotFound();
                    }
                }
                else{
                    return NotFound();
                }
                List<Usuario> usuariosEnBD = repoUsuario.GetAll();
                foreach (var usuario in usuariosEnBD)
                {
                    (tareaSelecVM.IdUsuarios).Add(usuario.Id);
                }
                return View(tareaSelecVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult AsignarTareaFromForm(AsignarTareaViewModel tareaSelecVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin()) return RedirectToAction("Index","Login");

                repoTarea.Assign(tareaSelecVM.Id,tareaSelecVM.IdUsuarioAsignado);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
        [HttpGet]
        public IActionResult CambiarEstadoTarea(int? idTarea){  
            try
            {
                if(!isLogin()) return RedirectToAction("Index","Login");

                Tarea tareaAEditar = repoTarea.GetById(idTarea);
                EditarTareaViewModel tareaAEditarVM = new EditarTareaViewModel();
                
                if (isAdmin()){
                    tareaAEditarVM = EditarTareaViewModel.FromTarea(tareaAEditar);
                }else if (idTarea.HasValue){
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if ((usuarioLogeado.Id == repoTarea.GetById(idTarea).IdUsuarioPropietario) || (usuarioLogeado.Id == repoTarea.GetById(idTarea).IdUsuarioAsignado)){
                        tareaAEditarVM = EditarTareaViewModel.FromTarea(tareaAEditar);
                    }else{
                        return NotFound();
                    }
                }else{
                    return NotFound();
                }
                return View(tareaAEditarVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult CambiarEstadoFromForm(EditarTareaViewModel tareaAEditarVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin()) return RedirectToAction("Index","Login"); 

                Tarea tareaAEditar = Tarea.FromEditarTareaViewModel(tareaAEditarVM);
                repoTarea.Update(tareaAEditar);
                return RedirectToAction("Index", new { idTablero = tareaAEditar.IdTablero });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            } 
        }
        private bool isAdmin()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("NivelDeAcceso") == "admin"){
                return true;
            }else{
                return false;
            }
        }
        private bool isLogin()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("NivelDeAcceso") == "admin" || HttpContext.Session.GetString("NivelDeAcceso") == "simple"){
                return true;
            }else{
                return false;
            }
        }
    }
}