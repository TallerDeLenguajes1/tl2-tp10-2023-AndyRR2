using Microsoft.AspNetCore.Mvc;//Necesario para eredar el Controller proporcionado

using Proyecto.Repositories;
using Proyecto.Models;
using Proyecto.ViewModels;

namespace Proyecto.Controllers{
    public class UsuarioController: Controller{
        private readonly string direccionBD;
        private readonly IUsuarioRepository repoUsuario;
        private readonly ILoginRepository repoLogin;
        private readonly ILogger<HomeController> _logger;
        public UsuarioController(ILogger<HomeController> logger, IUsuarioRepository usuRepo, string cadenaDeConexion, ILoginRepository logRepo) 
        {
            _logger = logger;
            repoUsuario = usuRepo;
            repoLogin = logRepo;
            direccionBD = cadenaDeConexion;
        }

        public IActionResult Index(){
            try
            {
                if(!isLogin()) return RedirectToAction("Index","Login"); 

                List<Usuario> usuarios = repoUsuario.GetAll();
                List<ListarUsuarioViewModel> listaUsuariosVM = ListarUsuarioViewModel.FromUsuario(usuarios);
                return View(listaUsuariosVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult AgregarUsuario(){
            try
            {
                if(!isLogin()) return RedirectToAction("Index","Login");
                if(!isAdmin()) return NotFound();

                CrearUsuarioViewModel newUsuarioVM = new CrearUsuarioViewModel();
                return View(newUsuarioVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult AgregarUsuarioFromForm(CrearUsuarioViewModel newUsuarioVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin()) return RedirectToAction("Index","Login"); 
                if(!isAdmin()) return NotFound();

                Usuario newUsuario = Usuario.FromCrearUsuario(newUsuarioVM);
                repoUsuario.Create(newUsuario);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult EditarUsuario(int? idUsuario){
            try
            {
                if(!isLogin()) return RedirectToAction("Index","Login"); 

                Usuario usuarioAEditar = repoUsuario.GetById(idUsuario);
                EditarUsuarioViewModel usuarioAEditarVM = new EditarUsuarioViewModel();

                if (isAdmin()){
                    usuarioAEditarVM = EditarUsuarioViewModel.FromUsuario(usuarioAEditar);
                }else if(idUsuario.HasValue){
                    //comparamos el id del usuario logueado con el del usuario que se quiere editar
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == idUsuario){
                        usuarioAEditarVM = EditarUsuarioViewModel.FromUsuario(usuarioAEditar);
                    }else{
                        return NotFound();
                    }
                }else{
                    return NotFound();
                }
                return View(usuarioAEditarVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult EditarUsuarioFromForm(EditarUsuarioViewModel usuarioAEditarVM){
            try
            { 
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin()) return RedirectToAction("Index","Login"); 
                if(usuarioAEditarVM.ContraseniaActual == HttpContext.Session.GetString("Contrasenia")){
                    Usuario usuarioAEditar = Usuario.FromEditarUsuario(usuarioAEditarVM);//convertir de EditarUsuarioViewModel a Usuario
                    repoUsuario.Update(usuarioAEditar);
                    return RedirectToAction("Index");
                }else{
                    _logger.LogInformation($"La contraseña ingresada es incorrecta");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult EliminarUsuario(int? idUsuario){
            try
            {
                if(!isLogin()) return RedirectToAction("Index","Login"); 

                Usuario usuarioAEliminar = repoUsuario.GetById(idUsuario);

                if (isAdmin()){
                    return View(usuarioAEliminar);
                }else if(idUsuario.HasValue){
                    //comparamos el id del usuario logueado con el del usuario que se quiere eliminar
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == idUsuario){
                        return View(usuarioAEliminar);
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
        public IActionResult EliminarFromForm(Usuario usuarioAEliminar){
            try
            {
                if(!isLogin()) return RedirectToAction("Index","Login"); 

                repoUsuario.Remove(usuarioAEliminar.Id);
                return RedirectToAction("Index");
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