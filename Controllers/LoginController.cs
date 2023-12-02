using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Tp11.Models;
using Tp11.ViewModels;

namespace Tp11.Controllers;

public class LoginController : Controller
{
    List<Login> listaDeTiposDelogins = new List<Login>();
    private readonly ILogger<LoginController> _logger;
    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;

        Login loginAdmin = new Login();
        loginAdmin.Nombre = "admin";
        loginAdmin.Contrasenia = "admin";
        loginAdmin.Nivel = NivelDeAcceso.admin;

        Login loginSimple = new Login();
        loginSimple.Nombre = "simple";
        loginSimple.Contrasenia = "simple";
        loginSimple.Nivel = NivelDeAcceso.simple;

        Login loginSimple2 = new Login();
        loginSimple.Nombre = "simple2";
        loginSimple.Contrasenia = "simple2";
        loginSimple.Nivel = NivelDeAcceso.simple;

        listaDeTiposDelogins.Add(loginAdmin);
        listaDeTiposDelogins.Add(loginSimple);
        listaDeTiposDelogins.Add(loginSimple2);
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
    public IActionResult Login(Login login)
    {
        try
        {
            Login usuarioPorLoguear=null;
            //existe el usuario?
            /*foreach (var usuario in listaDeTiposDelogins)
            {
                if (usuario.Nombre == login.Nombre && usuario.Contrasenia == login.Contrasenia)
                {
                    usuarioPorLoguear = login;
                }
            }*/
            usuarioPorLoguear = listaDeTiposDelogins.FirstOrDefault(u => u.Nombre == login.Nombre && u.Contrasenia == login.Contrasenia);

            // si el usuario no existe devuelvo al index, sino Registro el usuario
            if (usuarioPorLoguear == null){
                _logger.LogWarning($"Intento de acceso inválido - Usuario: {login.Nombre} Clave ingresada: {login.Contrasenia}");
                return RedirectToAction("Index");
            }else{
                _logger.LogInformation($"El usuario {usuarioPorLoguear.Nombre} ingresó correctamente");
                //Registro el usuario
                logearUsuario(usuarioPorLoguear);
                //Devuelvo el usuario al Home
                var rutaARedireccionar = new { controller = "Usuario", action = "Index" };//el tipo de var es un tipo anonimo
                return RedirectToRoute(rutaARedireccionar);
            } 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    private void logearUsuario(Login usuarioPorLoguear)
    {
        HttpContext.Session.SetString("Nombre", usuarioPorLoguear.Nombre);
        HttpContext.Session.SetString("Contrasenia", usuarioPorLoguear.Contrasenia);
        HttpContext.Session.SetString("NivelDeAcceso", Convert.ToString(usuarioPorLoguear.Nivel));
    }
}