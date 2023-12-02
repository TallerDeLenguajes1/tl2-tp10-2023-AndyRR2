/*
Para Proyecto Final únicamente.
Implemente la funcionalidad de asignar usuarios a tareas. El funcionamiento debería
ser el siguiente:
c. El usuario logueado debe poder asignar un usuario a las tareas de las que es
propietario.
d. El usuario logueado debería poder ver en la lista de tableros, además de los
tableros que le pertenecen, todos los tableros donde tenga tareas asignadas.
Los permisos del usuario logueado para tableros que no le pertenecen son:
i. Tableros: Solo lectura
ii. Tareas no asignadas: Solo lectura.
iii. Tareas asignadas: Lectura y mod
*/
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Tp11.Models;
using Tp11.ViewModels;
using EspacioTareaRepository;
using Microsoft.AspNetCore.Http.HttpResults;

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
        try
        {
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

            CrearTareaViewModel newTareaVM = new CrearTareaViewModel();
            return View(newTareaVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    [HttpPost]
    public IActionResult AgregarTareaFromForm([FromForm] CrearTareaViewModel newTareaVM){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index","Login");
            if(!isLogin()) return RedirectToAction("Index","Login");

            Tarea newTarea = Tarea.FromCrearTareaViewModel(newTareaVM);
            repo.Create(newTarea);
            return RedirectToAction("Index");
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
        EditarTareaViewModel tareaAEditarVM = EditarTareaViewModel.FromTarea(tareaAEditar);
        return View(tareaAEditarVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    [HttpPost]
    public IActionResult EditarTareaFromForm([FromForm] EditarTareaViewModel tareaAEditarVM){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index","Login");
            if(!isLogin()) return RedirectToAction("Index","Login"); 

            Tarea tareaAEditar = Tarea.FromEditarTareaViewModel(tareaAEditarVM);
            repo.Update(tareaAEditar);
            return RedirectToAction("Index");
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