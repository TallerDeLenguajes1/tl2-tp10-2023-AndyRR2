using System.Data.SQLite;

using Tp10.Models;

namespace EspacioUsuarioRepository{
    public class UsuariosRepository : IUsuariosRepository
    {
        private string cadenaConexion = "Data Source=C:/Taller_2/tl2-tp09-2023-AndyRR2/Tp8/kamban.db;Cache=Shared";
        public List<Usuario> GetAll(){
            var queryString = @"SELECT * FROM Usuario;";
            List<Usuario> usuarios = new List<Usuario>();
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                SQLiteCommand command = new SQLiteCommand(queryString, connection);
                connection.Open();
            
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var usuario = new Usuario();
                        usuario.Id = Convert.ToInt32(reader["id"]);
                        usuario.Nombre = reader["nombre_de_usuario"].ToString();
                        usuarios.Add(usuario);
                    }
                }
                connection.Close();
            }
            return usuarios;
        }

        public void Create(Usuario usuario){
            var query = $"INSERT INTO Usuario (id, nombre_de_usuario) VALUES (@Id,@name)";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@name", usuario.Nombre));
                command.Parameters.Add(new SQLiteParameter("@Id", usuario.Id));
                command.ExecuteNonQuery();

                connection.Close();   
            }
        }
        public Usuario GetById(int Id){
            var usuario = new Usuario();

            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Usuario WHERE id = @Id";
            command.Parameters.Add(new SQLiteParameter("@Id", Id));
            connection.Open();
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.Nombre= reader["nombre_de_usuario"].ToString();
                }
            }
            connection.Close();
            return(usuario);
        }

        public void Remove(int Id){
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            using (connection)
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                using (command)
                {
                    command.CommandText = "DELETE FROM Usuario WHERE id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();      
                }
                connection.Close();
            }
        }
        public void Update(Usuario usuario){
            string texto = "UPDATE Usuario SET nombre_de_usuario = @name WHERE id = @Id;";
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            using (connection)
            {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(texto, connection);
                    command.Parameters.Add(new SQLiteParameter("@name", usuario.Nombre));
                    command.Parameters.Add(new SQLiteParameter("@Id", usuario.Id));
                    command.ExecuteNonQuery();
                    connection.Close();  
            }
        }
    }
}