namespace Tp11.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;

using Tp11.Models;
using Tp11.ViewModels;
using EspacioTableroRepository;

public class TableroController : Controller{
    private readonly string direccionBD = "Data Source = DataBase/kamban.db;Cache=Shared";
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
            if(!isLogin()) return RedirectToAction("Index","Login");
            
            List<Tablero> tableros = null;
            
            if (isAdmin()){
                tableros = repo.GetAll();
            }else if(idUsuario.HasValue){

                int? ID = ObtenerIDDelUsuarioLogueado(direccionBD);
                if (ID == idUsuario){
                    tableros = repo.GetTablerosDeUsuario(ID);
                }else{
                    return NotFound();
                }

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
            if(!isAdmin()) return RedirectToAction("Index","Login");

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
            if(!isAdmin()) return RedirectToAction("Index","Login");

            Tablero newTablero = Tablero.FromTableroViewModel(newTableroVM);
            int? ID = newTablero.IdUsuarioPropietario;
            repo.Create(newTablero);
            return RedirectToAction("Index", new { idUsuario = ID });//redirecciona al index con el idDelUsuario en caso de que sea un usuario Simple
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
            TableroViewModel tableroAEditarVM = null;
            
            if (isAdmin()){
                 tableroAEditarVM = TableroViewModel.FromTablero(tableroAEditar);
            }else if(idTablero.HasValue){
                int? ID = ObtenerIDDelUsuarioLogueado(direccionBD);
                
                if (ID == tableroAEditar.IdUsuarioPropietario){
                    tableroAEditarVM = TableroViewModel.FromTablero(tableroAEditar);
                }else{
                    return NotFound();
                }
            }else{
                return NotFound();
            }
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

            Tablero tableroAEditar = Tablero.FromTableroViewModel(tableroAEditarVM);
            int? ID = tableroAEditar.IdUsuarioPropietario;
            repo.Update(tableroAEditar);
            return RedirectToAction("Index", new { idUsuario = ID });
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
            int? idUsuarioTablero = tableroAEliminar.IdUsuarioPropietario;
            
            if (isAdmin()){
                return View(tableroAEliminar);
            }else if(idTablero.HasValue){
                int? ID = ObtenerIDDelUsuarioLogueado(direccionBD);
                
                if (ID == idUsuarioTablero){
                    return View(tableroAEliminar);
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
    public IActionResult EliminarFromForm([FromForm] Tablero tableroAEliminar){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index","Login");
            if(!isLogin()) return RedirectToAction("Index","Login");
            
            repo.Remove(tableroAEliminar.Id);
            return RedirectToAction("Index", "Usuario");
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