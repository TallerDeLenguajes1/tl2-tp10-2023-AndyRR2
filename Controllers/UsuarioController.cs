using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Tp11.Models;
using EspacioUsuarioRepository;
using Tp11.ViewModels;

namespace Tp11.Controllers;

public class UsuarioController : Controller{
    UsuarioRepository repo = new UsuarioRepository();
    
    private readonly ILogger<HomeController> _logger;
    public UsuarioController(ILogger<HomeController> logger) //constructor de Usuario que recibe un parametro tipo ILogger<HomeController> 
    {
        _logger = logger;
    }

    public IActionResult Index(){
        if(!isLogin()) return RedirectToAction("Index","Login");

        List<ListarUsuarioViewModel> usuarios = repo.GetAll();
        return View(usuarios);
    }

    [HttpGet]
    public IActionResult AgregarUsuario(){
        if(!isLogin()) return RedirectToAction("Index","Login"); 

        CrearUsuarioViewModel newUsuario = new CrearUsuarioViewModel();
        return View(newUsuario);
    }
    [HttpPost]
    public IActionResult AgregarUsuarioFromForm([FromForm] CrearUsuarioViewModel newUsuario){
        if(!ModelState.IsValid) return RedirectToAction("Index","Login");
        if(!isLogin()) return RedirectToAction("Index","Login"); 

        repo.Create(newUsuario);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarUsuario(int? idUsuario){
        if(!isLogin()) return RedirectToAction("Index","Login"); 

        Usuario usuarioAEditar = repo.GetById(idUsuario);
        return View(usuarioAEditar);
    }
    [HttpPost]
    public IActionResult EditarUsuarioFromForm([FromForm] Usuario usuarioAEditar){
        if(!ModelState.IsValid) return RedirectToAction("Index","Login");
        if(!isLogin()) return RedirectToAction("Index","Login"); 

        repo.Update(usuarioAEditar);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EliminarUsuario(int? idUsuario){
        if(!isLogin()) return RedirectToAction("Index","Login"); 

        Usuario usuarioAEliminar = repo.GetById(idUsuario);
        return View(usuarioAEliminar);
    }
    [HttpPost]
    public IActionResult EliminarFromForm(Usuario usuarioAEliminar){
        if(!isLogin()) return RedirectToAction("Index","Login"); 
        
        repo.Remove(usuarioAEliminar.Id);
        return RedirectToAction("Index");
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}