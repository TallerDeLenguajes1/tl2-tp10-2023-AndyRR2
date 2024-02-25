using Microsoft.AspNetCore.Mvc;//Necesario para heredar el Controller proporcionado

using Proyecto.Repositories;
using Proyecto.Models;
using Proyecto.ViewModels;

namespace Proyecto.Controllers{
    public class TableroController: Controller{
        private readonly ITableroRepository repoTablero;
        private readonly IUsuarioRepository repoUsuario;
        private readonly ILoginRepository repoLogin;
        private readonly ILogger<HomeController> _logger;
        public TableroController(ILogger<HomeController> logger, ITableroRepository tabRepo, IUsuarioRepository usuRepo, ILoginRepository logRepo) 
        {
            _logger = logger;
            repoTablero = tabRepo;
            repoUsuario = usuRepo;
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

                if (isAdmin()){
                    if (idUsuario.HasValue){//OBS: se podria dividir en dos Index uno con parametro y otro vacio********************
                        tableros = repoTablero.GetAllByOwnerUser(idUsuario).Union(repoTablero.GetAllByAsignedTask(idUsuario)).GroupBy(t => t.Id).Select(group => group.First()).ToList();
                    }else{
                        tableros = repoTablero.GetAll();
                    }
                }else{
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (idUsuario == usuarioLogeado.Id){
                        tableros = repoTablero.GetAllByOwnerUser(idUsuario).Union(repoTablero.GetAllByAsignedTask(idUsuario)).GroupBy(t => t.Id).Select(group => group.First()).ToList();   
                    }else{
                        _logger.LogWarning("Debe ser administrador para realizar la accion");
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
                if(!isAdmin()){
                    _logger.LogWarning("Debe ser administrador para realizar la accion");
                    return NotFound();
                } 

                CrearTableroViewModel newTableroVM = new CrearTableroViewModel();//Obtiene usuarios a seleccionar
                newTableroVM.Usuarios = repoUsuario.GetAll();
                
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
                if(!isAdmin()){
                    _logger.LogWarning("Debe ser administrador para realizar la accion");
                    return NotFound();
                } 

                Tablero newTablero = Tablero.FromCrearTableroViewModel(newTableroVM);
                repoTablero.Create(newTablero);
                return RedirectToAction("Index", new { idUsuario = newTablero.Propietario.Id });//Redirecciona al index con el idDelUsuario
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
                    if (usuarioLogeado.Id == repoTablero.GetById(idTablero).Propietario.Id){
                        tableroAEditarVM = EditarTableroViewModel.FromTablero(tableroAEditar);//Convierto de Model a ViewModel
                    }else{
                        _logger.LogWarning("Debe ser administrador para realizar la accion");
                        return NotFound();
                    }
                }

                tableroAEditarVM.Usuarios = repoUsuario.GetAll();//Obtiene usuarios a seleccionar
    
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
                return RedirectToAction("Index", new { idUsuario = tableroAEditar.Propietario.Id});
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
                    if (usuarioLogeado.Id == repoTablero.GetById(idTablero).Propietario.Id){
                        return View(tableroAEliminar);
                    }else{
                        _logger.LogWarning("Debe ser administrador para realizar la accion");
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
                return false;
            }
        }
        private bool isLogin()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("NivelDeAcceso") == "admin" || HttpContext.Session.GetString("NivelDeAcceso") == "simple"){
                return true;
            }else{
                _logger.LogWarning("Debe estar logueado para ingresar a la página");
                return false;
            }
        }
    }
}