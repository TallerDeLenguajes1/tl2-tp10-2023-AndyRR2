using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using EspacioTareaRepository;
using Tp10.Models;
namespace Tp10.Controllers;

public class TareaController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static List<Tarea> tareas = new List<Tarea>();
    TareasRepository repo;

    public TareaController(ILogger<HomeController> logger)
    {
        _logger = logger;
        repo = new TareasRepository();
    }

    //Mostrar Tareas
    public IActionResult Index()
    {
        var tareas = repo.GetAll();
        return View(tareas);
    }
    //..
    
    //Agregar Tarea
    [HttpGet]
    public IActionResult AgregarTarea()
    {
        return View(new Tarea());
    }

    [HttpPost]
    public IActionResult AgregarTareaFromForm([FromForm] Tarea tarea)
    {
        repo.Create(tarea);
        return RedirectToAction("Index");
    }
    //..
    
    //Editar Tarea
    [HttpGet]
    public IActionResult EditarTarea(int idTarea)
    {  
        return View(repo.GetById(idTarea));
    }

    [HttpPost]
    public IActionResult EditarTareaFromForm([FromForm] Tarea tarea)
    {  
        repo.Update(tarea);
        return RedirectToAction("Index");
    }
    //..
    //Eliminar Tarea
    public IActionResult DeleteTarea(int idTarea)
    {
        return View(repo.GetById(idTarea));
    }

    [HttpPost]
    public IActionResult DeleteFromForm([FromForm] Tarea tarea)
    {
        repo.Remove(tarea.Id);
        return RedirectToAction("Index");
    }
    //..






    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}