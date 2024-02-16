using Microsoft.AspNetCore.Mvc;//Necesario para eredar el Controller proporcionado

using Proyecto.Repositories;
using Proyecto.Models;
using Proyecto.ViewModels;

namespace Proyecto.Controllers{
    public class TableroController: Controller{
        private readonly string direccionBD;
        private readonly ITableroRepository repoTablero;
        private readonly IUsuarioRepository repoUsuario;
        private readonly ITareaRepository repoTarea;
        private readonly ILoginRepository repoLogin;
        private readonly ILogger<HomeController> _logger;
        public TableroController(ILogger<HomeController> logger, ITableroRepository tabRepo, string cadenaDeConexion, IUsuarioRepository usuRepo, ITareaRepository tarRepo, ILoginRepository logRepo) 
        {
            _logger = logger;
            repoTablero = tabRepo;
            repoUsuario = usuRepo;
            repoTarea = tarRepo;
            repoLogin = logRepo;
            direccionBD = cadenaDeConexion;
        }

        public IActionResult Index(int? idUsuario){
            try
            {
                if(!isLogin()) return RedirectToAction("Index","Login"); 
                
                List<Tablero> tableros = new List<Tablero>();

                if (isAdmin()){
                    if (idUsuario.HasValue){
                        tableros = repoTablero.GetByOwnerUser(idUsuario).Concat(repoTablero.GetByUserAsignedTask(idUsuario)).GroupBy(t => t.Id).Select(group => group.First()).ToList();
                    }else{
                        tableros = repoTablero.GetAll();
                    }
                }else if(idUsuario.HasValue){
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if ((idUsuario == usuarioLogeado.Id)){
                        tableros = repoTablero.GetByOwnerUser(usuarioLogeado.Id).Concat(repoTablero.GetByUserAsignedTask(usuarioLogeado.Id)).GroupBy(t => t.Id).Select(group => group.First()).ToList();   
                    }else{
                        return NotFound();
                    }
                }else{
                    return NotFound();
                }
                
                List<ListarTableroViewModel> listaTablerosVM = ListarTableroViewModel.FromTablero(tableros);
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
                if(!isAdmin()) return NotFound();

                CrearTableroViewModel newTableroVM = new CrearTableroViewModel();
                List<Usuario> usuariosEnBD = repoUsuario.GetAll();
                foreach (var usuario in usuariosEnBD)
                {
                    (newTableroVM.IdUsuarios).Add(usuario.Id);
                }

                return View(newTableroVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult AgregarTableroFromForm(CrearTableroViewModel newTableroVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin()) return RedirectToAction("Index","Login");
                if(!isAdmin()) return NotFound();

                Tablero newTablero = Tablero.FromCrearTableroViewModel(newTableroVM);
                repoTablero.Create(newTablero);
                return RedirectToAction("Index", new { idUsuario = newTablero.IdUsuarioPropietario });//redirecciona al index con el idDelUsuario
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

                Tablero tableroAEditar = repoTablero.GetById(idTablero);
                EditarTableroViewModel tableroAEditarVM = new EditarTableroViewModel();
                
                if (isAdmin()){
                    tableroAEditarVM = EditarTableroViewModel.FromTablero(tableroAEditar);
                }else if (idTablero.HasValue){
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == repoTablero.GetById(idTablero).IdUsuarioPropietario){
                        tableroAEditarVM = EditarTableroViewModel.FromTablero(tableroAEditar);
                    }else{
                        return NotFound();
                    }
                }else{
                    return NotFound();
                }

                List<Usuario> usuariosEnBD = repoUsuario.GetAll();
                foreach (var usuario in usuariosEnBD)
                {
                    (tableroAEditarVM.IdUsuarios).Add(usuario.Id);
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
        public IActionResult EditarTableroFromForm([FromForm] EditarTableroViewModel tableroAEditarVM){
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index","Login");
                if(!isLogin()) return RedirectToAction("Index","Login");

                Tablero tableroAEditar = Tablero.FromEditarTableroViewModel(tableroAEditarVM);
                repoTablero.Update(tableroAEditar);
                return RedirectToAction("Index", new { idUsuario = tableroAEditar.IdUsuarioPropietario });
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
                
                Tablero tableroAEliminar = repoTablero.GetById(idTablero);

                if (isAdmin()){
                    return View(tableroAEliminar);
                }else if (idTablero.HasValue){
                    Usuario usuarioLogeado = repoLogin.ObtenerUsuario(HttpContext.Session.GetString("Nombre"),HttpContext.Session.GetString("Contrasenia"));
                    if (usuarioLogeado.Id == repoTablero.GetById(idTablero).IdUsuarioPropietario){
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
                
                repoTablero.Remove(tableroAEliminar.Id);
                return RedirectToAction("Index", "Usuario");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
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
    }
}