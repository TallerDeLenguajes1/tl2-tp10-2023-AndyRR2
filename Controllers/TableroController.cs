using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using EspacioTableroRepository;
using Tp10.Models;
namespace Tp10.Controllers;

public class TableroController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static List<Tablero> tableros = new List<Tablero>();
    TablerosRepository repo;

    public TableroController(ILogger<HomeController> logger)
    {
        _logger = logger;
        repo = new TablerosRepository();
    }

    //Mostrar Tableros
    public IActionResult Index()
    {
        var tableros = repo.GetAll();
        return View(tableros);
    }
    //..
    
    //Agregar Tablero
    [HttpGet]
    public IActionResult AgregarTablero()
    {
        return View(new Tablero());
    }

    [HttpPost]
    public IActionResult AgregarTableroFromForm([FromForm] Tablero tablero)
    {
        repo.Create(tablero);
        return RedirectToAction("Index");
    }
    //..
    
    //Editar Tablero
    [HttpGet]
    public IActionResult EditarTablero(int idTablero)
    {  
        return View(repo.GetById(idTablero));
    }

    [HttpPost]
    public IActionResult EditarTableroFromForm([FromForm] Tablero tablero)
    {  
        repo.Update(tablero);
        return RedirectToAction("Index");
    }
    //..

    
    //Eliminar Tablero
    public IActionResult DeleteTablero(int idTablero)
    {
        return View(repo.GetById(idTablero));
    }

    [HttpPost]
    public IActionResult DeleteFromForm([FromForm] Tablero tablero)
    {
        repo.Remove(tablero.Id);
        return RedirectToAction("Index");
    }
    //..






    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}