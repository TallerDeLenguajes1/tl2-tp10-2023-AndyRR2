using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;

using Tp11.Models;
using Tp11.ViewModels;

namespace Tp11.Controllers;

public class LoginController : Controller
{
    private readonly string direccionBD = "Data Source = DataBase/kamban.db;Cache=Shared";
    //List<Login> listaDeTiposDelogins = new List<Login>();
    private readonly ILogger<LoginController> _logger;
    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
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
    public IActionResult Login(Login login)//endpoint de control de acceso
    {
        try
        {
            bool validacion = false;
            Login usuarioPorLoguear = new Login();
            
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "SELECT * FROM Usuario WHERE contrasenia = @PASS AND nombre_de_usuario = @USER";
            SQLiteParameter parameterUser = new SQLiteParameter("@USER", login.Nombre);
            SQLiteParameter parameterPass = new SQLiteParameter("@PASS", login.Contrasenia);

            using (connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterUser);
                commandC.Parameters.Add(parameterPass);

                SQLiteDataReader readerC = commandC.ExecuteReader();
                using (readerC)
                {
                    while (readerC.Read())
                    {
                        validacion = true;
                        usuarioPorLoguear.Contrasenia = Convert.ToString(readerC["contrasenia"]);
                        usuarioPorLoguear.Nombre = Convert.ToString(readerC["nombre_de_usuario"]);
                        usuarioPorLoguear.Nivel = (NivelDeAcceso)Enum.Parse(typeof(NivelDeAcceso), Convert.ToString(readerC["nivel_de_acceso"]), true); //convierte de string a enum
                    }
                }
                connectionC.Close();
            }

            // si el usuario no existe devuelvo al index, sino Registro el usuario
            if (validacion == false){
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