using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Tp11.Models;
using EspacioTableroRepository;

namespace Tp11.Controllers;
using Tp11.ViewModels;

public class TableroController : Controller{
    //TableroRepository repo = new TableroRepository();
    private readonly ITableroRepository repo;
    private readonly ILogger<HomeController> _logger;
    public TableroController(ILogger<HomeController> logger, ITableroRepository TabRepo) //constructor de Tablero que recibe un parametro tipo ILogger<HomeController> 
    {
        _logger = logger;
        repo = TabRepo;
    }

    public IActionResult Index(int? idUsuario){
        List<Tablero> tableros = null;
        if(!isLogin()) return RedirectToAction("Index","Login");
        
        if (isAdmin()){
            tableros = repo.GetAll();
        }else if(idUsuario.HasValue){
            tableros = repo.GetTablerosDeUsuario(idUsuario);//ver como poner el parametro adecuado segun el id del usuario logueado
        }else{
            return NotFound();
        }
        List<ListarTableroViewModel> listaTablerosVM = ListarTableroViewModel.FromTablero(tableros);
        return View(listaTablerosVM);
    }

    [HttpGet]
    public IActionResult AgregarTablero(){
        if(!isLogin()) return RedirectToAction("Index","Login");

        CrearTableroViewModel newTableroVM = new CrearTableroViewModel();
        return View(newTableroVM);
    }
    [HttpPost]
    public IActionResult AgregarTableroFromForm([FromForm] CrearTableroViewModel newTableroVM){
        if(!ModelState.IsValid) return RedirectToAction("Index","Login");
        if(!isLogin()) return RedirectToAction("Index","Login");

        Tablero newTablero = Tablero.FromCrearTableroViewModel(newTableroVM);
        repo.Create(newTablero);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarTablero(int? idTablero){
        if(!isLogin()) return RedirectToAction("Index","Login");

        Tablero tableroAEditar = repo.GetById(idTablero);
        EditarTableroViewModel tableroAEditarVM = EditarTableroViewModel.FromTablero(tableroAEditar);
        return View(tableroAEditarVM);
    }
    [HttpPost]
    public IActionResult EditarTableroFromForm([FromForm] EditarTableroViewModel tableroAEditarVM){
        if(!ModelState.IsValid) return RedirectToAction("Index","Login");
        if(!isLogin()) return RedirectToAction("Index","Login");

        Tablero tableroAEditar = Tablero.FromEditarTableroViewModel(tableroAEditarVM);
        repo.Update(tableroAEditar);
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public IActionResult EliminarTablero(int? idTablero){
        if(!isLogin()) return RedirectToAction("Index","Login");

        Tablero tableroAEliminar = repo.GetById(idTablero);
        return View(tableroAEliminar);
    }
    [HttpPost]
    public IActionResult EliminarFromForm(Tablero tableroAEliminar){
        if(!isLogin()) return RedirectToAction("Index","Login");
        
        repo.Remove(tableroAEliminar.Id);
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