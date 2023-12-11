namespace Tp11.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;

using Tp11.Models;
using Tp11.ViewModels;
using EspacioTareaRepository;

public class TareaController : Controller{
    private readonly string direccionBD = "Data Source = DataBase/kamban.db;Cache=Shared";
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
            if(!isLogin()) return RedirectToAction("Index","Login");

            List<Tarea> tareas = null;
            
            if (isAdmin()){
                tareas = repo.GetAll();
            }else if(idTablero.HasValue){

                int? ID = ObtenerIDDelUsuarioLogueado(direccionBD);
                if (repo.GetById(idTablero).IdUsuarioAsignado == null){
                    tareas = repo.GetTareasDeTablero(idTablero);
                }else if (ID == repo.GetById(idTablero).IdUsuarioAsignado){
                    tareas = repo.GetTareasDeTablero(idTablero);
                }else{
                    return NotFound();
                }
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
            if(!isAdmin()) return NotFound();

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
            if(!isAdmin()) return NotFound();

            Tarea newTarea = Tarea.FromTareaViewModel(newTareaVM);
            int? ID = newTarea.IdTablero;
            repo.Create(newTarea);
            return RedirectToAction("Index", new { iTablero = ID });
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
            TareaViewModel tareaAEditarVM = null;
            int? ID = ObtenerIDDelUsuarioLogueado(direccionBD);

            tareaAEditarVM = TareaViewModel.FromTarea(tareaAEditar);
            if (isAdmin())
            {
                return View(tareaAEditarVM);
            }else if(idTarea.HasValue)
            {
                if (ID == tareaAEditar.IdUsuarioPropietario)
                {
                    return View(tareaAEditarVM);
                }else if (ID == tareaAEditar.IdUsuarioAsignado)
                {
                    return View("EditarTareaSimple",tareaAEditarVM);
                }else
                {
                    return NotFound();
                }
            }else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    /*[HttpGet]
    public IActionResult EditarTareaSimple(int? idTarea){  
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login");

            Tarea tareaAEditar = repo.GetById(idTarea);
            TareaViewModel tareaAEditarVM = null;
            int? ID = ObtenerIDDelUsuarioLogueado(direccionBD);

            tareaAEditarVM = TareaViewModel.FromTarea(tareaAEditar);
            if (isAdmin())
            {
                return View(tareaAEditarVM);
            }else if(idTarea.HasValue)
            {
                if (ID == tareaAEditar.IdUsuarioPropietario)
                {
                    return View(tareaAEditarVM);
                }else if (ID == tareaAEditar.IdUsuarioAsignado)
                {
                    return View("EditarTareaSimple",tareaAEditarVM.Id);
                }else
                {
                    return NotFound();
                }
            }else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }*/
    [HttpPost]
    public IActionResult EditarTareaFromForm([FromForm] TareaViewModel tareaAEditarVM){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index","Login");
            if(!isLogin()) return RedirectToAction("Index","Login"); 

            Tarea tareaAEditar = Tarea.FromTareaViewModel(tareaAEditarVM);
            int? ID = tareaAEditar.IdTablero;
            repo.Update(tareaAEditar);
            return RedirectToAction("Index", new { idTablero = ID });
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
            int? idUsuarioTarea = tareaAEliminar.IdUsuarioAsignado;
            
            if (isAdmin()){
                return View(tareaAEliminar);
            }else if(idTarea.HasValue){
                int? ID = ObtenerIDDelUsuarioLogueado(direccionBD);
                
                if (ID == idUsuarioTarea){
                    return View(tareaAEliminar);
                }else{
                    return NotFound();
                }
            }else{
                return NotFound();
            }
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
            if(!isAdmin()) return NotFound();

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
            if(!isAdmin()) return NotFound();

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
    private int? ObtenerIDDelUsuarioLogueado(string? direccionBD){// se agrego para poder hacer el control cuando se seleccione editar/agregar/eliminar tableros que no son del usuario logueado y este no es Admin
        int? ID = 0;
        Usuario usuarioSelec = new Usuario();
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

        string queryC = "SELECT * FROM Usuario WHERE nombre_de_usuario = @NAME AND contrasenia = @PASS";
        SQLiteParameter parameterName = new SQLiteParameter("@NAME", HttpContext.Session.GetString("Nombre"));
        SQLiteParameter parameterPass = new SQLiteParameter("@PASS", HttpContext.Session.GetString("Contrasenia"));

        using (connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterName);
            commandC.Parameters.Add(parameterPass);
            
            SQLiteDataReader readerC = commandC.ExecuteReader();
            using (readerC)
            {
                while (readerC.Read())
                {
                    ID = Convert.ToInt32(readerC["id"]);
                }
            }
            connectionC.Close();
        }
        return(ID);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}