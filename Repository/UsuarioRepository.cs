namespace EspacioUsuarioRepository;

using System.Data.SQLite;
using Tp11.Models;

public class UsuarioRepository : IUsuarioRepository{
    private readonly string direccionBD = "Data Source = DataBase/kamban.db;Cache=Shared";

    public void Create(Usuario newUsuario){
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
        if (newUsuario==null){
            throw new Exception("El Usuario no se creo correctamente.");
        }
    }
    public void Update(Usuario newUsuario){
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
        
        string queryC = "UPDATE Usuario SET nombre_de_usuario = @NAME, contrasenia = @PASS, nivel_de_acceso = @NIVEL WHERE id = @ID";
        SQLiteParameter parameterId = new SQLiteParameter("@ID",newUsuario.Id);
        SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",newUsuario.Nombre);
        SQLiteParameter parameterPass = new SQLiteParameter("@PASS",newUsuario.Contrasenia);
        SQLiteParameter parameterNivel = new SQLiteParameter("@NIVEL",newUsuario.Nivel);

        using (connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterId);
            commandC.Parameters.Add(parameterNombre);
            commandC.Parameters.Add(parameterPass);
            commandC.Parameters.Add(parameterNivel);
            
            int rowsAffected = commandC.ExecuteNonQuery();
            connectionC.Close();
            if (rowsAffected == 0){
                throw new Exception("No se encontró ningún usuario con el ID proporcionado.");
            }
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
        if (usuarioSelec==null){
            throw new Exception("El usuario no esta creado.");
        }
        return(usuarioSelec);
    }
    public List<Usuario> GetAll(){
        List<Usuario> usuarios = new List<Usuario>();
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
                    Usuario usuarioPorAgregar = new Usuario();
                    usuarioPorAgregar.Id = Convert.ToInt32(readerC["id"]);
                    usuarioPorAgregar.Nombre = Convert.ToString(readerC["nombre_de_usuario"]);
                    usuarios.Add(usuarioPorAgregar);
                }   
            }
            connectionC.Close();
        }
        if (usuarios==null){
            throw new Exception("Lista de usuarios no encontrada.");
        }
        return(usuarios);
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
            
            int rowsAffected = commandC.ExecuteNonQuery();
            connectionC.Close();
            if (rowsAffected == 0){
                throw new Exception("No se encontró ningún usuario con el ID proporcionado.");
            }
        }
    }
}