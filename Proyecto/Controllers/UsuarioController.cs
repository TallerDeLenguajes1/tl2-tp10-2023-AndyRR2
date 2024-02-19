using Microsoft.AspNetCore.Mvc;//Necesario para heredar el Controller proporcionado

using Proyecto.Repositories;
using Proyecto.Models;
using Proyecto.ViewModels;

namespace Proyecto.Controllers{
    public class UsuarioController: Controller{
        private readonly IUsuarioRepository repoUsuario;
        private readonly ILoginRepository repoLogin;
        private readonly ILogger<HomeController> _logger;
        public UsuarioController(ILogger<HomeController> logger, IUsuarioRepository usuRepo, ILoginRepository logRepo) 
        {
            _logger = logger;
            repoUsuario = usuRepo;
            repoLogin = logRepo;
        }

        public IActionResult Index(){
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                List<Usuario> listaUsuarios = repoUsuario.GetAll();
                List<ListarUsuarioViewModel> listaUsuariosVM = ListarUsuarioViewModel.FromUsuario(listaUsuarios);

                return View(listaUsuariosVM);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método Index del controlador de Usuario: {ex.ToString()}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult AgregarUsuario(){
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                } 
                if(!isAdmin()) return NotFound();

                CrearUsuarioViewModel newUsuarioVM = new CrearUsuarioViewModel();

                return View(newUsuarioVM);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método AgregarUsuario del controlador de Usuario: {ex.ToString()}");
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult AgregarUsuarioFromForm(CrearUsuarioViewModel newUsuarioVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                } 
                if(!isAdmin()) return NotFound();

                Usuario newUsuario = Usuario.FromCrearUsuario(newUsuarioVM);
                repoUsuario.Create(newUsuario);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método AgregarUsuarioFromForm del controlador de Usuario: {ex.ToString()}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult EditarUsuario(int? idUsuario){
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                } 

                if(!idUsuario.HasValue) return NotFound();//Verifica que tenga un Valor asignado
                Usuario usuarioAEditar = repoUsuario.GetById(idUsuario);//Obtengo el usuario de la DB con el Modelo base
                EditarUsuarioViewModel usuarioAEditarVM = new EditarUsuarioViewModel();//Instancia inicial del ViewModel
                
                if (isAdmin()){//Si es Admin puede editarlo
                    usuarioAEditarVM = EditarUsuarioViewModel.FromUsuario(usuarioAEditar);//Convierto de Model a ViewModel
                }
                else{
                    //Verifica si el id del usuario logueado es el mismo que el del usuario que se quiere editar
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == idUsuario){//Si coinciden los Id puede editarlo
                        usuarioAEditarVM = EditarUsuarioViewModel.FromUsuario(usuarioAEditar);//Convierto de Model a ViewModel
                    }else{
                        return NotFound();
                    }
                }

                return View(usuarioAEditarVM);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método EditarUsuario del controlador de Usuario: {ex.ToString()}");
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult EditarUsuarioFromForm(EditarUsuarioViewModel usuarioAEditarVM){
            try
            { 
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                //Verifica si la contraseña Actual ingresada coincide con la del mismo usuario en la DB
                if(usuarioAEditarVM.ContraseniaActual == repoUsuario.GetById(usuarioAEditarVM.Id).Contrasenia){
                    Usuario usuarioAEditar = Usuario.FromEditarUsuario(usuarioAEditarVM);//Convierto de ViewModel a Model
                    repoUsuario.Update(usuarioAEditar);
                    return RedirectToAction("Index");
                }else{
                    _logger.LogInformation($"La contraseña ingresada es incorrecta");
                    return RedirectToAction("EditarUsuario", new { idUsuario = usuarioAEditarVM.Id });//Regresa a EditarUsuario con el mismo Id del usuario que se queria editar
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método EditarUsuarioFromForm del controlador de Usuario: {ex.ToString()}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult EliminarUsuario(int? idUsuario){
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                if(!idUsuario.HasValue) return NotFound();
                Usuario usuarioAEliminar = repoUsuario.GetById(idUsuario);//Obtengo el usuario por su Id

                if (isAdmin()){//Si es Admin puede Borrarlo
                    return View(usuarioAEliminar);
                }else{
                    //Verifica si el id del usuario logueado es el mismo que el del usuario que se quiere Borrar
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == idUsuario){//Si coinciden los Id puede Borrarlo
                        return View(usuarioAEliminar);
                    }else{
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método EliminarUsuario del controlador de Usuario: {ex.ToString()}");
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult EliminarFromForm(int? idUsuarioAEliminar){
            try
            {
                if(!isLogin())
                {
                    TempData["Mensaje"] = "Debe iniciar sesión para acceder a esta página.";
                    return RedirectToAction("Index", "Login");
                }

                repoUsuario.Remove(idUsuarioAEliminar);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud en el método EliminarUsuarioFromForm del controlador de Usuario: {ex.ToString()}");
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