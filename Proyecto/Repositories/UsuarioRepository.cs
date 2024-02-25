using System.Data.SQLite;

using Proyecto.Models;

namespace Proyecto.Repositories{
    public class UsuarioRepository: IUsuarioRepository{
        private readonly string direccionBD;
        public UsuarioRepository(string cadenaDeConexion)
        {
            direccionBD = cadenaDeConexion;
        }
        public List<Usuario> GetAll(){
            List<Usuario> usuarios = new List<Usuario>();
            
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "SELECT * FROM Usuario;";

            using(connectionC)
            {
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
                        usuarioPorAgregar.NivelDeAcceso = (NivelDeAcceso)Convert.ToInt16(readerC["nivel_de_acceso"]);
                        usuarios.Add(usuarioPorAgregar);
                    }   
                }
                connectionC.Close();
            }
            if (usuarios.Count == 0)
            {
                throw new Exception("No se encontraron usuarios en la base de datos.");
            }
            return(usuarios);
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
                        usuarioSelec.Contrasenia = Convert.ToString(readerC["contrasenia"]);
                        usuarioSelec.NivelDeAcceso = (NivelDeAcceso)Convert.ToInt16(readerC["nivel_de_acceso"]);
                    }
                }
                connectionC.Close();
            }
            if (usuarioSelec==null){
                throw new Exception("No se encontro el usuario con el id proporcionado en la base de datos.");
            }
            return(usuarioSelec);
        }
        public void Create(Usuario newUsuario){
            if (UserExists(newUsuario.Nombre))
            {
                throw new Exception("El Usuario ya existe.");
            }
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            
            string queryC = $"INSERT INTO Usuario (nombre_de_usuario, contrasenia, nivel_de_acceso) VALUES (@NAME,@PASS,@NIVEL)";
            SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",newUsuario.Nombre);
            SQLiteParameter parameterPass = new SQLiteParameter("@PASS",newUsuario.Contrasenia);
            SQLiteParameter parameterNivel = new SQLiteParameter("@NIVEL",newUsuario.NivelDeAcceso);

            using (connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterNombre);
                commandC.Parameters.Add(parameterPass);
                commandC.Parameters.Add(parameterNivel);

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
            SQLiteParameter parameterNivel = new SQLiteParameter("@NIVEL",newUsuario.NivelDeAcceso);

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
        public void Remove(int? idUsuario){

            /*foreach (var tarea in repoTarea.GetByOwnerUser(idUsuario))//inhabilita todas las tareas del usuario a borrar
            {
                repoTarea.Disable(tarea.Id,tarea.IdTablero);
            }*/

            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            
            string queryC = "DELETE FROM Usuario WHERE id = @ID";
            SQLiteParameter parameterId = new SQLiteParameter("@ID",idUsuario);

            using(connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterId);
                
                int rowsAffected = commandC.ExecuteNonQuery();
                
                if (rowsAffected == 0){
                    throw new Exception("No se encontró ningún usuario con el ID proporcionado.");
                }
                connectionC.Close();
            }
        }

        public bool UserExists(string? nombreUsuario){
            bool validacion=false;
            string? Nombre=null;
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "SELECT * FROM Usuario WHERE nombre_de_usuario = @NAME";
            SQLiteParameter parameterName = new SQLiteParameter("@NAME",nombreUsuario);

            using (connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterName);
                
                SQLiteDataReader readerC = commandC.ExecuteReader();
                using (readerC)
                {
                    while (readerC.Read())
                    {
                        Nombre = Convert.ToString(readerC["nombre_de_usuario"]);
                    }
                }
                connectionC.Close();
            }
            if (Nombre!=null){
                validacion=true;
            }
            return validacion;
        }
    }
}