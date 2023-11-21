using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Tp11.Models;
using EspacioTareaRepository;

namespace Tp11.Controllers;

public class TareaController : Controller{
    TareaRepository repo = new TareaRepository();
    
    private readonly ILogger<HomeController> _logger;
    public TareaController(ILogger<HomeController> logger)
    {
        _logger = logger;
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
        return View(tareas);
    }
    
    [HttpGet]
    public IActionResult AgregarTarea(){
        if(!isLogin()) return RedirectToAction("Index","Login");

        Tarea newTarea = new Tarea();
        return View(newTarea);
    }
    [HttpPost]
    public IActionResult AgregarTareaFromForm([FromForm] Tarea newTarea){
        if(!ModelState.IsValid) return RedirectToAction("Index","Login");
        if(!isLogin()) return RedirectToAction("Index","Login");

        repo.Create(newTarea);
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public IActionResult EditarTarea(int? idTarea){  
        if(!isLogin()) return RedirectToAction("Index","Login");

        Tarea tareaAEditar = repo.GetById(idTarea);
        return View(tareaAEditar);
    }
    [HttpPost]
    public IActionResult EditarTareaFromForm([FromForm] Tarea tareaAEditar){ 
        if(!ModelState.IsValid) return RedirectToAction("Index","Login");
        if(!isLogin()) return RedirectToAction("Index","Login"); 

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