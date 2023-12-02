using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Tp11.Models;
using EspacioUsuarioRepository;
using Tp11.ViewModels;

namespace Tp11.Controllers;

public class UsuarioController : Controller{
    //UsuarioRepository repo = new UsuarioRepository();
    private readonly IUsuarioRepository repo;
    private readonly ILogger<HomeController> _logger;
    public UsuarioController(ILogger<HomeController> logger, IUsuarioRepository UsuRepo) //constructor de Usuario que recibe un parametro tipo ILogger<HomeController> 
    {
        _logger = logger;
        repo = UsuRepo;
    }

    public IActionResult Index(){
        try
        {
            /*var rutaARedireccionar = new { controller = "Login", action = "Index" };//el tipo de var es un tipo anonimo
            return RedirectToRoute(rutaARedireccionar);*/ //tambien es valido para redireccionar
            if(!isLogin()) return RedirectToAction("Index","Login");

            List<Usuario> usuarios = repo.GetAll();
            List<ListarUsuarioViewModel> listaUsuariosVM = ListarUsuarioViewModel.FromUsuario(usuarios);//convertir de List<Usuario> a List<listarUsuarioViewModel>
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
    public IActionResult AgregarUsuarioFromForm([FromForm] CrearUsuarioViewModel newUsuarioVM){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index","Login");
            if(!isLogin()) return RedirectToAction("Index","Login");

            Usuario newUsuario = Usuario.FromCrearUsuarioViewModel(newUsuarioVM);//convertir de CrearUsuarioViewModel a Usuario
            repo.Create(newUsuario);
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

            Usuario usuarioAEditar = repo.GetById(idUsuario);
            EditarUsuarioViewModel editarUsuarioVM = EditarUsuarioViewModel.FromUsuario(usuarioAEditar);//convertir de Usuario a EditarUsuarioViewModel
            return View(editarUsuarioVM);
        }
        catch (Exception ex)
        {
            
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    [HttpPost]
    public IActionResult EditarUsuarioFromForm([FromForm] EditarUsuarioViewModel usuarioAEditarVM){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index","Login");
            //RedirectToRoute (new {}).....
            if(!isLogin()) return RedirectToAction("Index","Login"); 

            Usuario usuarioAEditar = Usuario.FromEditarUsuarioViewModel(usuarioAEditarVM);//convertir de EditarUsuarioViewModel a Usuario
            repo.Update(usuarioAEditar);
            return RedirectToAction("Index");
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

            Usuario usuarioAEliminar = repo.GetById(idUsuario);
            return View(usuarioAEliminar);
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
        
            repo.Remove(usuarioAEliminar.Id);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    //preguntar si los try catch van en los de abajo tambien
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