using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using EspacioUsuarioRepository;
using Tp10.Models;
namespace Tp10.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static List<Usuario> usuarios = new List<Usuario>();
    UsuariosRepository repo;

    public UsuarioController(ILogger<HomeController> logger)
    {
        _logger = logger;
        repo = new UsuariosRepository();
    }

    //Mostrar Usuarios
    public IActionResult Index()
    {
        var usuarios = repo.GetAll();
        return View(usuarios);
    }
    //..
    
    //Agregar Usuario
    [HttpGet]
    public IActionResult AgregarUsuario()
    {
        return View(new Usuario());
    }

    [HttpPost]
    public IActionResult AgregarUsuarioFromForm([FromForm] Usuario usuario)
    {
        repo.Create(usuario);
        return RedirectToAction("Index");
    }
    //..

    //Editar Usuario
    [HttpGet]
    public IActionResult EditarUsuario(int idUsuario)
    {  
        return View(repo.GetById(idUsuario));
    }

    [HttpPost]
    public IActionResult EditarUsuarioFromForm([FromForm] Usuario usuario)
    {  
        repo.Update(usuario);
        return RedirectToAction("Index");
    }
    //..

    //Eliminar Usuario
    public IActionResult DeleteUsuario(int idUsuario)
    {
        return View(repo.GetById(idUsuario));
    }

    [HttpPost]
    public IActionResult EliminarForm(Usuario user)
    {
        repo.Remove(user.Id);
        return RedirectToAction("Index");
    }
    //..






    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}