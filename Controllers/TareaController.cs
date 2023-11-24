using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Tp11.Models;
using Tp11.ViewModels;
using EspacioTareaRepository;

namespace Tp11.Controllers;

public class TareaController : Controller{
    //TareaRepository repo = new TareaRepository();
    private readonly ITareaRepository repo;
    private readonly ILogger<HomeController> _logger;
    public TareaController(ILogger<HomeController> logger, ITareaRepository TarRepo)
    {
        _logger = logger;
        repo = TarRepo;
    }

    public IActionResult Index(int? idTablero){
        List<Tarea> tareas = null;
        if(!isLogin()) return RedirectToAction("Index","Login");
        
        if (isAdmin()){
            tareas = repo.GetAll();
        }else if(idTablero.HasValue){
            tareas = repo.GetTareasDeTablero(idTablero);//ver como poner el parametro adecuado segun el id del usuario logueado
        }else{
            return NotFound();
        }
        List<ListarTareaViewModel> listaTareasVM = ListarTareaViewModel.FromTarea(tareas);
        return View(listaTareasVM);
    }
    
    [HttpGet]
    public IActionResult AgregarTarea(){
        if(!isLogin()) return RedirectToAction("Index","Login");

        CrearTareaViewModel newTareaVM = new CrearTareaViewModel();
        return View(newTareaVM);
    }
    [HttpPost]
    public IActionResult AgregarTareaFromForm([FromForm] CrearTareaViewModel newTareaVM){
        if(!ModelState.IsValid) return RedirectToAction("Index","Login");
        if(!isLogin()) return RedirectToAction("Index","Login");

        Tarea newTarea = Tarea.FromCrearTareaViewModel(newTareaVM);
        repo.Create(newTarea);
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public IActionResult EditarTarea(int? idTarea){  
        if(!isLogin()) return RedirectToAction("Index","Login");

        Tarea tareaAEditar = repo.GetById(idTarea);
        EditarTareaViewModel tareaAEditarVM = EditarTareaViewModel.FromTarea(tareaAEditar);
        return View(tareaAEditarVM);
    }
    [HttpPost]
    public IActionResult EditarTareaFromForm([FromForm] EditarTareaViewModel tareaAEditarVM){ 
        if(!ModelState.IsValid) return RedirectToAction("Index","Login");
        if(!isLogin()) return RedirectToAction("Index","Login"); 

        Tarea tareaAEditar = Tarea.FromEditarTareaViewModel(tareaAEditarVM);
        repo.Update(tareaAEditar);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EliminarTarea(int? idTarea){
        if(!isLogin()) return RedirectToAction("Index","Login");

        Tarea tareaAEliminar = repo.GetById(idTarea);
        return View(tareaAEliminar);
    }
    [HttpPost]
    public IActionResult EliminarFromForm([FromForm] Tarea tareaAEliminar){
        if(!isLogin()) return RedirectToAction("Index","Login");

        repo.Remove(tareaAEliminar.Id);
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