using System.Data.SQLite;

namespace EspacioUsuarioRepository;
using Tp11.Models;
using Tp11.ViewModels;

public class UsuarioRepository : IUsuarioRepository{
    private string direccionBD = "Data Source = C:/Taller_2/Tp10-11/tl2-tp10-2023-AndyRR2/DataBase/kamban.db;Cache=Shared";
    //private string direccionBD = "./DataBase/kamban.db;Cache=Shared";
    public List<ListarUsuarioViewModel> GetAll(){
        List<ListarUsuarioViewModel> usuarios = new List<ListarUsuarioViewModel>();
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

        string queryC = "SELECT * FROM Usuario;";
        
        using(connectionC){
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            
            SQLiteDataReader readerC = commandC.ExecuteReader();
            using (readerC)
            {
                while (readerC.Read())
                {
                    ListarUsuarioViewModel usuarioPorAgregar = new ListarUsuarioViewModel();
                    usuarioPorAgregar.Id = Convert.ToInt32(readerC["id"]);
                    usuarioPorAgregar.Nombre = Convert.ToString(readerC["nombre_de_usuario"]);
                    usuarios.Add(usuarioPorAgregar);
                }   
            }
            connectionC.Close();
        }
        return(usuarios);
    }
    public void Create(CrearUsuarioViewModel newUsuario){
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
        
        string queryC = $"INSERT INTO Usuario (id, nombre_de_usuario) VALUES (@ID,@NAME)";
        SQLiteParameter parameterId = new SQLiteParameter("@ID",newUsuario.Id);
        SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",newUsuario.Nombre);

        using (connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterId);
            commandC.Parameters.Add(parameterNombre);

            commandC.ExecuteNonQuery();
            connectionC.Close();
        }
    }
    public Usuario GetById(int? Id){
        Usuario usuarioSelec = new Usuario();
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

        string queryC = "SELECT * FROM Usuario WHERE id = @ID";
        SQLiteParameter parameterId = new SQLiteParameter("@ID", Id);
        
        using (connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterId);
            
            SQLiteDataReader readerC = commandC.ExecuteReader();
            using (readerC)
            {
                while (readerC.Read())
                {
                    usuarioSelec.Id = Convert.ToInt32(readerC["id"]);
                    usuarioSelec.Nombre = Convert.ToString(readerC["nombre_de_usuario"]);
                }
            }
            connectionC.Close();
        }
        return(usuarioSelec);
    }
    public void Remove(int? idUsuario){
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

        string queryC = "DELETE FROM Usuario WHERE id = @ID";
        SQLiteParameter parameterId = new SQLiteParameter("@ID",idUsuario);
        
        using(connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterId);
            commandC.ExecuteNonQuery();
            connectionC.Close();
        }
    }
    public void Update(EditarUsuarioViewModel newUsuario){
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
        
        string queryC = "UPDATE Usuario SET nombre_de_usuario = @NAME WHERE id = @ID";
        SQLiteParameter parameterId = new SQLiteParameter("@ID",newUsuario.Id);
        SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",newUsuario.Nombre);

        using (connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterId);
            commandC.Parameters.Add(parameterNombre);
            
            commandC.ExecuteNonQuery();
            connectionC.Close();
        }
    }
}