namespace Tp11.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;

using Tp11.Models;
using Tp11.ViewModels;
using EspacioUsuarioRepository;

public class UsuarioController : Controller{
    private readonly string direccionBD = "Data Source = DataBase/kamban.db;Cache=Shared";
    private readonly IUsuarioRepository repo;
    private readonly ILogger<HomeController> _logger;
    public UsuarioController(ILogger<HomeController> logger, IUsuarioRepository UsuRepo) //constructor de Usuario que recibe un parametro tipo ILogger<HomeController> 
    {
        _logger = logger;
        repo = UsuRepo;
    }

    public IActionResult Index(){
        try
        {
            /*var rutaARedireccionar = new { controller = "Login", action = "Index" };//el tipo de var es un tipo anonimo
            return RedirectToRoute(rutaARedireccionar);*/ //tambien es valido para redireccionar
            if(!isLogin()) return RedirectToAction("Index","Login");

            List<Usuario> usuarios = repo.GetAll();
            List<ListarUsuarioViewModel> listaUsuariosVM = ListarUsuarioViewModel.FromUsuario(usuarios);//convertir de List<Usuario> a List<listarUsuarioViewModel>
            return View(listaUsuariosVM);
        }
        catch (Exception ex)
        {
            
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult AgregarUsuario(){
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login");

            UsuarioViewModel newUsuarioVM = new UsuarioViewModel();
            return View(newUsuarioVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    [HttpPost]
    public IActionResult AgregarUsuarioFromForm([FromForm] UsuarioViewModel newUsuarioVM){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index","Login");
            if(!isLogin()) return RedirectToAction("Index","Login");

            Usuario newUsuario = Usuario.FromUsuarioViewModel(newUsuarioVM);//convertir de CrearUsuarioViewModel a Usuario
            repo.Create(newUsuario);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult EditarUsuario(int? idUsuario){
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login"); 

            Usuario usuarioAEditar = repo.GetById(idUsuario);
            UsuarioViewModel editarUsuarioVM = null;

            if (isAdmin()){
                editarUsuarioVM = UsuarioViewModel.FromUsuario(usuarioAEditar);
            }else if(idUsuario.HasValue){
                int? ID = ObtenerIDDelUsuarioLogueado(direccionBD);
                if (ID == idUsuario){
                    editarUsuarioVM = UsuarioViewModel.FromUsuario(usuarioAEditar);
                }else{
                    return NotFound();
                }
            }else{
                return NotFound();
            }
            return View(editarUsuarioVM);
        }
        catch (Exception ex)
        {
            
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    [HttpPost]
    public IActionResult EditarUsuarioFromForm([FromForm] UsuarioViewModel usuarioAEditarVM){
        try
        {
            if(!ModelState.IsValid) return RedirectToAction("Index","Login");
            //RedirectToRoute (new {}).....
            if(!isLogin()) return RedirectToAction("Index","Login"); 
            
            Usuario usuarioAEditar = Usuario.FromUsuarioViewModel(usuarioAEditarVM);//convertir de EditarUsuarioViewModel a Usuario
            repo.Update(usuarioAEditar);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult EliminarUsuario(int? idUsuario){
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login"); 
            Usuario usuarioAEliminar = repo.GetById(idUsuario);
            
            if (isAdmin()){
                return View(usuarioAEliminar);
            }else if(idUsuario.HasValue){
                int? ID = ObtenerIDDelUsuarioLogueado(direccionBD);
                
                if (ID == idUsuario){
                    return View(usuarioAEliminar);
                }else{
                    return NotFound();
                }
            }else{
                return NotFound();
            }
            return View(usuarioAEliminar);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return BadRequest();
        }
    }
    [HttpPost]
    public IActionResult EliminarFromForm(Usuario usuarioAEliminar){
        try
        {
            if(!isLogin()) return RedirectToAction("Index","Login"); 
            
            repo.Remove(usuarioAEliminar.Id);
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