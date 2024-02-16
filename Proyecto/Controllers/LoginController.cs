using Microsoft.AspNetCore.Mvc;//Necesario para eredar el Controller proporcionado

using Proyecto.ViewModels;
using Proyecto.Models;
using Proyecto.Repositories;

namespace Proyecto.Controllers{
    public class LoginController: Controller{
        private readonly ILoginRepository repoLogin;
        private readonly ILogger<HomeController> _logger;
        public LoginController(ILogger<HomeController> logger, ILoginRepository logRepo) 
        {
            _logger = logger;
            repoLogin = logRepo;
        }

        public IActionResult Index()
        {
            try
            {
                LoginViewModel login = new LoginViewModel();
                return View(login);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
        
        [HttpPost]
        public IActionResult ValidarUsuario(LoginViewModel login)
        {
            if(repoLogin.AutenticarUsuario(login.Nombre,login.Contrasenia)){
                Usuario usuarioPorLoguear = repoLogin.ObtenerUsuario(login.Nombre,login.Contrasenia);
                
                _logger.LogInformation($"El usuario {usuarioPorLoguear.Nombre} ingresó correctamente");

                logearUsuario(usuarioPorLoguear);
                var rutaARedireccionar = new { controller = "Usuario", action = "Index" };
                return RedirectToRoute(rutaARedireccionar);
                
            }else{
                _logger.LogWarning($"Intento de acceso inválido - Usuario: {login.Nombre} Clave ingresada: {login.Contrasenia}");
                return RedirectToAction("Index");
            }
        }

        private void logearUsuario(Usuario usuarioPorLoguear)
        {
            HttpContext.Session.SetString("Nombre", usuarioPorLoguear.Nombre);
            HttpContext.Session.SetString("Contrasenia", usuarioPorLoguear.Contrasenia);
            HttpContext.Session.SetString("NivelDeAcceso", Convert.ToString(usuarioPorLoguear.NivelDeAcceso));
        }

        public IActionResult Desloguear()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index"); // Redirige al usuario a la página de inicio, o a donde sea apropiado en tu aplicación
        }
    }
}