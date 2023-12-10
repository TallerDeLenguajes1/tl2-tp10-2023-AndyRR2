namespace Tp11.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Tp11.Models;
using Tp11.ViewModels;
using EspacioTareaRepository;

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
        try
        {
            List<Tarea> tareas = null;
            if(!isLogin()) return RedirectToAction("Index","Login");
            
            if (isAdmin()){
                tareas = repo.GetAll();
            }else if(idTablero.HasValue){
                tareas = repo.GetTareasDeTablero(idTablero);
            }else{
                return NotFound();
            }
            List<TareaViewModel> listaTareasVM = TareaViewModel.FromTarea(tareas);
            return View(listaTareasVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    
    [HttpGet]
    public IActionResult AgregarTarea(){
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login");

            TareaViewModel newTareaVM = new TareaViewModel();
            return View(newTareaVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    [HttpPost]
    public IActionResult AgregarTareaFromForm([FromForm] TareaViewModel newTareaVM){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index","Login");
            if(!isLogin()) return RedirectToAction("Index","Login");

            Tarea newTarea = Tarea.FromTareaViewModel(newTareaVM);
            int? ID = newTarea.IdUsuarioAsignado;
            repo.Create(newTarea);
            return RedirectToAction("Index", new { idUsuario = ID });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    
    [HttpGet]
    public IActionResult EditarTarea(int? idTarea){  
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login");

            Tarea tareaAEditar = repo.GetById(idTarea);
            TareaViewModel tareaAEditarVM = TareaViewModel.FromTarea(tareaAEditar);
            return View(tareaAEditarVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    [HttpPost]
    public IActionResult EditarTareaFromForm([FromForm] TareaViewModel tareaAEditarVM){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index","Login");
            if(!isLogin()) return RedirectToAction("Index","Login"); 

            Tarea tareaAEditar = Tarea.FromTareaViewModel(tareaAEditarVM);
            int? ID = tareaAEditar.IdUsuarioAsignado;
            repo.Update(tareaAEditar);
            return RedirectToAction("Index", new { idUsuario = ID });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        } 
    }

    [HttpGet]
    public IActionResult EliminarTarea(int? idTarea){
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login");

            Tarea tareaAEliminar = repo.GetById(idTarea);
            return View(tareaAEliminar);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        } 
    }
    [HttpPost]
    public IActionResult EliminarFromForm([FromForm] Tarea tareaAEliminar){
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login");

            repo.Remove(tareaAEliminar.Id);
            return RedirectToAction("Index", "Usuario");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    [HttpGet]
    public IActionResult AsignarTareaAUsuario(int idTarea){
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login");

            Tarea tareaSelec = repo.GetById(idTarea);
            TareaViewModel tareaSelecVM = TareaViewModel.FromTarea(tareaSelec);
            return View(tareaSelecVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    [HttpPost]
    public IActionResult AsignarTareaAUsuarioFromForm([FromForm] TareaViewModel tareaSelecVM){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index","Login");
            if(!isLogin()) return RedirectToAction("Index","Login");

            Tarea tareaSelec = Tarea.FromTareaViewModel(tareaSelecVM);
            repo.AsignarUsuario(tareaSelec);
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