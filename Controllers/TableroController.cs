namespace Tp11.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Tp11.Models;
using Tp11.ViewModels;
using EspacioTableroRepository;

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
        try
        {
            List<Tablero> tableros = null;
            if(!isLogin()) return RedirectToAction("Index","Login");
            
            if (isAdmin()){
                tableros = repo.GetAll();
            }else if(idUsuario.HasValue){
                tableros = repo.GetTablerosDeUsuario(idUsuario);//ver como poner el parametro adecuado segun el id del usuario logueado
            }else{
                return NotFound();
            }
            List<TableroViewModel> listaTablerosVM = TableroViewModel.FromTablero(tableros);
            return View(listaTablerosVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult AgregarTablero(){
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login");

            TableroViewModel newTableroVM = new TableroViewModel();
            return View(newTableroVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    [HttpPost]
    public IActionResult AgregarTableroFromForm([FromForm] TableroViewModel newTableroVM){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index","Login");
            if(!isLogin()) return RedirectToAction("Index","Login");

            Tablero newTablero = Tablero.FromCrearTableroViewModel(newTableroVM);
            repo.Create(newTablero);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult EditarTablero(int? idTablero){
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login");

            Tablero tableroAEditar = repo.GetById(idTablero);
            TableroViewModel tableroAEditarVM = TableroViewModel.FromTablero(tableroAEditar);
            return View(tableroAEditarVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
        
    }
    [HttpPost]
    public IActionResult EditarTableroFromForm([FromForm] TableroViewModel tableroAEditarVM){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index","Login");
            if(!isLogin()) return RedirectToAction("Index","Login");

            Tablero tableroAEditar = Tablero.FromEditarTableroViewModel(tableroAEditarVM);
            repo.Update(tableroAEditar);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    
    [HttpGet]
    public IActionResult EliminarTablero(int? idTablero){
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login");

            Tablero tableroAEliminar = repo.GetById(idTablero);
            return View(tableroAEliminar);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
        
    }
    [HttpPost]
    public IActionResult EliminarFromForm(Tablero tableroAEliminar){
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login");
        
            repo.Remove(tableroAEliminar.Id);
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