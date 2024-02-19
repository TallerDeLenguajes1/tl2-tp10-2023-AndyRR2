using Microsoft.AspNetCore.Mvc;//Necesario para heredar el Controller proporcionado

using Proyecto.Repositories;
using Proyecto.Models;
using Proyecto.ViewModels;

namespace Proyecto.Controllers{
    public class TableroController: Controller{
        private readonly ITableroRepository repoTablero;
        private readonly IUsuarioRepository repoUsuario;
        private readonly ITareaRepository repoTarea;
        private readonly ILoginRepository repoLogin;
        private readonly ILogger<HomeController> _logger;
        public TableroController(ILogger<HomeController> logger, ITableroRepository tabRepo, IUsuarioRepository usuRepo, ITareaRepository tarRepo, ILoginRepository logRepo) 
        {
            _logger = logger;
            repoTablero = tabRepo;
            repoUsuario = usuRepo;
            repoTarea = tarRepo;
            repoLogin = logRepo;
        }

        public IActionResult Index(int? idUsuario){
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                List<Tablero> tableros = new List<Tablero>();

                if (isAdmin()){//Si es Admin puede ver todos los tableros
                    /*Si el idUsuario tiene un Valor o es Nulo obtiene los tableros propiedad de ese usuario
                    y los une con los tableros donde ese usuario tenga alguna tarea asignada.
                    ***Se hace énfasis en idUsuario.HasValue ya que segun los enlaces del Index de Usario se puede acceder a...
                    los tableros de un usuario seleccionado o a TODOS los tableros de todos los usuarios, y segun lo que se elija
                    el parámetro en la ruta tendra un Valor cuando se selecciona al usuario en especifico por lo tanto solo se mostraran
                    los tableros de ese usuario, o sera un texto "null" si se selecciona ver todos los Tableros*/
                    if (idUsuario.HasValue){
                        tableros = repoTablero.GetByOwnerUser(idUsuario).Concat(repoTablero.GetByUserAsignedTask(idUsuario)).GroupBy(t => t.Id).Select(group => group.First()).ToList();
                    }else{
                        tableros = repoTablero.GetAll();
                    }
                }else{
                    /*Si no es Admin solo puede acceder a ver sus propios Tableros los cuales son los tableros 
                    propiedad del usuario unidos con los tableros donde el usuario tenga alguna tarea asignada.*/
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if ((idUsuario == usuarioLogeado.Id)){
                        tableros = repoTablero.GetByOwnerUser(usuarioLogeado.Id).Concat(repoTablero.GetByUserAsignedTask(usuarioLogeado.Id)).GroupBy(t => t.Id).Select(group => group.First()).ToList();   
                    }else{
                        return NotFound();
                    }
                }
    
                List<ListarTableroViewModel> listaTablerosVM = ListarTableroViewModel.FromTablero(tableros);
                return View(listaTablerosVM);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método Index del controlador de Tablero: {ex.ToString()}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult AgregarTablero(){
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }
                if(!isAdmin()) return NotFound();

                CrearTableroViewModel newTableroVM = new CrearTableroViewModel();
                List<Usuario> usuariosEnBD = repoUsuario.GetAll();
                foreach (var usuario in usuariosEnBD)//Obtiene las lista de Id de Usuarios disponibles para seleccionar
                {
                    (newTableroVM.IdUsuarios).Add(usuario.Id);
                }

                return View(newTableroVM);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método AgregarTablero del controlador de Tablero: {ex.ToString()}");
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult AgregarTableroFromForm(CrearTableroViewModel newTableroVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }
                if(!isAdmin()) return NotFound();

                Tablero newTablero = Tablero.FromCrearTableroViewModel(newTableroVM);
                repoTablero.Create(newTablero);
                return RedirectToAction("Index", new { idUsuario = newTablero.IdUsuarioPropietario });//Redirecciona al index con el idDelUsuario
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método AgregarTableroFromFrom del controlador de Tablero: {ex.ToString()}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult EditarTablero(int? idTablero){
            try
            {   
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                if(!idTablero.HasValue) return NotFound();//Verifica que tenga un Valor asignado
                Tablero tableroAEditar = repoTablero.GetById(idTablero);//Obtengo el tablero de la DB con el Modelo base
                EditarTableroViewModel tableroAEditarVM = new EditarTableroViewModel();//Instancia inicial del ViewModel
                
                if (isAdmin()){//Si es Admin puede editarlo
                    tableroAEditarVM = EditarTableroViewModel.FromTablero(tableroAEditar);
                }else{
                    //Verifica si el id del usuario logueado es el mismo que el del usuario propietario del tablero que se quiere editar
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == repoTablero.GetById(idTablero).IdUsuarioPropietario){
                        tableroAEditarVM = EditarTableroViewModel.FromTablero(tableroAEditar);//Convierto de Model a ViewModel
                    }else{
                        return NotFound();
                    }
                }

                List<Usuario> usuariosEnBD = repoUsuario.GetAll();
                foreach (var usuario in usuariosEnBD)//Obtiene las lista de Id de Usuarios disponibles para seleccionar
                {
                    (tableroAEditarVM.IdUsuarios).Add(usuario.Id);
                }
                return View(tableroAEditarVM);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método EditarTablero del controlador de Tablero: {ex.ToString()}");
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult EditarTableroFromForm(EditarTableroViewModel tableroAEditarVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                Tablero tableroAEditar = Tablero.FromEditarTableroViewModel(tableroAEditarVM);
                repoTablero.Update(tableroAEditar);
                return RedirectToAction("Index", new { idUsuario = tableroAEditar.IdUsuarioPropietario });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método EditarTableroFromForm del controlador de Tablero: {ex.ToString()}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult EliminarTablero(int? idTablero){
            try
            {   
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }
                
                if(!idTablero.HasValue) return NotFound();
                Tablero tableroAEliminar = repoTablero.GetById(idTablero);

                if (isAdmin()){//Si es Admin puede Borrarlo
                    return View(tableroAEliminar);
                }else{
                    //Verifica si el id del usuario logueado es el mismo que el del usuario propietario del tablero que se quiere Borrar
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == repoTablero.GetById(idTablero).IdUsuarioPropietario){
                        return View(tableroAEliminar);
                    }else{
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método EliminarTablero del controlador de Tablero: {ex.ToString()}");
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult EliminarTableroFromForm(int? idTableroAEliminar){
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }
                
                repoTablero.Remove(idTableroAEliminar);
                return RedirectToAction("Index", "Usuario");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método EliminarTableroFromForm del controlador de Tablero: {ex.ToString()}");
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