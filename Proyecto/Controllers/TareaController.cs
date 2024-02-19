using Microsoft.AspNetCore.Mvc;//Necesario para eredar el Controller proporcionado

using Proyecto.Repositories;
using Proyecto.Models;
using Proyecto.ViewModels;

namespace Proyecto.Controllers{
    public class TareaController: Controller{
        private readonly ITareaRepository repoTarea;
        private readonly IUsuarioRepository repoUsuario;
        private readonly ITableroRepository repoTablero;
        private readonly ILoginRepository repoLogin;
        private readonly ILogger<HomeController> _logger;
        public TareaController(ILogger<HomeController> logger, ITareaRepository tareRepo, IUsuarioRepository usuRepo, ITableroRepository tabRepo, ILoginRepository logRepo) 
        {
            _logger = logger;
            repoTarea = tareRepo;
            repoUsuario = usuRepo;
            repoTablero = tabRepo;
            repoLogin = logRepo;
        }

        public IActionResult Index(int? idTablero){
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                } 

                List<Tarea> tareas = new List<Tarea>();

                if(isAdmin()){//Si es Admin puede ver todas las tareas
                    /*Si el idTablero tiene un Valor o es Nulo obtiene las tareas de ese tablero
                    ***Se hace énfasis en idUsuario.HasValue ya que segun los enlaces del Index de Tablero se puede acceder a...
                    las tareas de un tablero seleccionado o a TODAS las tareas de todos los Tableros, y segun lo que se elija,
                    el parámetro en la ruta tendra un Valor cuando se selecciona un tablero en especifico por lo tanto solo se mostraran
                    las tareas de ese tablero, o sera un texto "null" si se selecciona ver todas las Tareas*/
                    if (idTablero.HasValue){
                        tareas = repoTarea.GetByOwnerBoard(idTablero);
                    }else{
                        tareas = repoTarea.GetAll();
                    }
                }else{
                    /*Si no es Admin solo puede acceder a ver las tareas que pertenezcan a los Tableros donde
                    estos tableros sean propiedad del usuario logueado o tengan alguna tarea asignada al usuario*/
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if ((repoTablero.GetById(idTablero).IdUsuarioPropietario == usuarioLogeado.Id) || repoTarea.ChechAsignedTask(idTablero,usuarioLogeado.Id)){
                        tareas = repoTarea.GetByOwnerBoard(idTablero);   
                    }else{
                        return NotFound();
                    }
                }

                List<ListarTareaViewModel> listaTareasVM = ListarTareaViewModel.FromTarea(tareas);
                return View(listaTareasVM);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método Index del controlador de Tarea: {ex.ToString()}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult AgregarTarea(){
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }
                if(!isAdmin()) return NotFound();

                CrearTareaViewModel newTareaVM = new CrearTareaViewModel();

                List<Usuario> usuariosEnBD = repoUsuario.GetAll();
                foreach (var usuario in usuariosEnBD)//Obtiene las lista de Id de Usuarios disponibles para seleccionar
                {
                    (newTareaVM.IdUsuarios).Add(usuario.Id);
                }

                List<Tablero> tablerosEnBD = repoTablero.GetAll();
                foreach (var tablero in tablerosEnBD)//Obtiene las lista de Id de Tableros disponibles para seleccionar
                {
                    (newTareaVM.IdTableros).Add(tablero.Id);
                }
                if((newTareaVM.IdTableros).Count == 0){//Si no hay tableros no puede crear Tareas
                    TempData["Mensaje"] = "No hay tableros donde puede agregarce la tarea.";
                    return RedirectToAction("Index", "Usuario");
                }
                return View(newTareaVM);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método AgregarTarea del controlador de Tarea: {ex.ToString()}");
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult AgregarTareaFromForm(CrearTareaViewModel newTareaVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }
                if(!isAdmin()) return NotFound();

                Tarea newTarea = Tarea.FromCrearTareaViewModel(newTareaVM);
                repoTarea.Create(newTarea);
                return RedirectToAction("Index", new { iTablero = newTarea.IdTablero });//Redirecciona al index con el idDelTablero
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método AgregarTareaFromForm del controlador de Tarea: {ex.ToString()}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult EditarTarea(int? idTarea){  
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                if(!idTarea.HasValue) return NotFound();
                Tarea tareaAEditar = repoTarea.GetById(idTarea);
                EditarTareaViewModel tareaAEditarVM = new EditarTareaViewModel();
                
                if (isAdmin()){//Si es Admin puede editarla
                    tareaAEditarVM = EditarTareaViewModel.FromTarea(tareaAEditar);
                }else{
                    //Verifica si el id del usuario logueado es el mismo que el del usuario propietario de la Tarea que se quiere editar
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == repoTarea.GetById(idTarea).IdUsuarioPropietario){
                        tareaAEditarVM = EditarTareaViewModel.FromTarea(tareaAEditar);//Convierto de Model a ViewModel
                    }else{
                        return NotFound();
                    }
                }

                List<Tablero> tablerosEnBD = repoTablero.GetAll();
                foreach (var tablero in tablerosEnBD)//Obtiene las lista de Id de Tableros disponibles para seleccionar
                {
                    (tareaAEditarVM.IdTableros).Add(tablero.Id);
                }
                return View(tareaAEditarVM);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método EditarTarea del controlador de Tarea: {ex.ToString()}");
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult EditarTareaFromForm(EditarTareaViewModel tareaAEditarVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                Tarea tareaAEditar = Tarea.FromEditarTareaViewModel(tareaAEditarVM);
                repoTarea.Update(tareaAEditar);
                return RedirectToAction("Index", new { idTablero = tareaAEditar.IdTablero });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método EditarTareaFromForm del controlador de Tarea: {ex.ToString()}");
                return BadRequest();
            } 
        }

        [HttpGet]
        public IActionResult EliminarTarea(int? idTarea){
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                if(!idTarea.HasValue) return NotFound();
                Tarea tareaAEliminar = repoTarea.GetById(idTarea);

                if (isAdmin()){//Si es Admin puede Borrarla
                    return View(tareaAEliminar);
                }else{
                    //Verifica si el id del usuario logueado es el mismo que el del usuario propietario de la Tarea que se quiere Borrar
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == repoTarea.GetById(idTarea).IdUsuarioPropietario){
                        return View(tareaAEliminar);
                    }else{
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método EliminarTarea del controlador de Tarea: {ex.ToString()}");
                return BadRequest();
            } 
        }
        [HttpPost]
        public IActionResult EliminarTareaFromForm(int? idTareaAEliminar){
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                repoTarea.Remove(idTareaAEliminar);
                return RedirectToAction("Index", "Usuario");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método EliminarTarea del controlador de Tarea: {ex.ToString()}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult AsignarTarea(int? idTarea){
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                if(!idTarea.HasValue) return NotFound();
                Tarea tareaSelec = repoTarea.GetById(idTarea);
                AsignarTareaViewModel tareaSelecVM = new AsignarTareaViewModel();

                if (isAdmin()){//Si es Admin puede Asignarla
                    tareaSelecVM = AsignarTareaViewModel.FromTarea(tareaSelec);
                }else{
                    //Verifica que el id del usuario logueado sea el mismo que el del usuario propietario de la Tarea a Asignar
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == repoTarea.GetById(idTarea).IdUsuarioPropietario){
                        tareaSelecVM = AsignarTareaViewModel.FromTarea(tareaSelec);
                    }else{
                        return NotFound();
                    }
                }
                
                List<Usuario> usuariosEnBD = repoUsuario.GetAll();
                foreach (var usuario in usuariosEnBD)//Obtiene las lista de Id de Usuarios disponibles para seleccionar
                {
                    (tareaSelecVM.IdUsuarios).Add(usuario.Id);
                }
                return View(tareaSelecVM);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método AsignarTarea del controlador de Tarea: {ex.ToString()}");
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult AsignarTareaFromForm(AsignarTareaViewModel tareaSelecVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                repoTarea.Assign(tareaSelecVM.Id,tareaSelecVM.IdUsuarioAsignado);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método AsignarTareaFromForm del controlador de Tarea: {ex.ToString()}");
                return BadRequest();
            }
        }
        [HttpGet]
        public IActionResult CambiarEstadoTarea(int? idTarea){  
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                if(!idTarea.HasValue) return NotFound();
                Tarea tareaAEditar = repoTarea.GetById(idTarea);
                EditarTareaViewModel tareaAEditarVM = new EditarTareaViewModel();
                
                if (isAdmin()){//Si es Admin puede Cambiar el Estado
                    tareaAEditarVM = EditarTareaViewModel.FromTarea(tareaAEditar);
                }else{
                    //Verifica que el id del usuario logueado sea el mismo que el usuario Propietario o el del usuario Asignado a la Tarea
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if ((usuarioLogeado.Id == repoTarea.GetById(idTarea).IdUsuarioPropietario) || (usuarioLogeado.Id == repoTarea.GetById(idTarea).IdUsuarioAsignado)){
                        tareaAEditarVM = EditarTareaViewModel.FromTarea(tareaAEditar);
                    }else{
                        return NotFound();
                    }
                }

                return View(tareaAEditarVM);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método CambiarEstadoTarea del controlador de Tarea: {ex.ToString()}");
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult CambiarEstadoFromForm(EditarTareaViewModel tareaAEditarVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                Tarea tareaAEditar = Tarea.FromEditarTareaViewModel(tareaAEditarVM);
                repoTarea.Update(tareaAEditar);
                return RedirectToAction("Index", new { idTablero = tareaAEditar.IdTablero });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método CambiarEstadoFromForm del controlador de Tarea: {ex.ToString()}");
                return BadRequest();
            } 
        }
        private bool isAdmin()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("NivelDeAcceso") == "admin"){
                return true;
            }else{
                _logger.LogWarning("Debe estar logueado para ingresar a la página");
                return false;
            }
        }
        private bool isLogin()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("NivelDeAcceso") == "admin" || HttpContext.Session.GetString("NivelDeAcceso") == "simple"){
                return true;
            }else{
                _logger.LogWarning("Debe ser administrador para realizar la accion");
                return false;
            }
        }
    }
}