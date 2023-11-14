using System.Data.SQLite;

using Tp10.Models;

namespace EspacioTableroRepository{
    public class TablerosRepository : ITablerosRepository
    {
        private string cadenaConexion = "Data Source=C:/Taller_2/tl2-tp09-2023-AndyRR2/Tp8/kamban.db;Cache=Shared";
        public List<Tablero> GetAll(){
            var queryString = @"SELECT * FROM Tablero;";
            List<Tablero> tableros = new List<Tablero>();
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                SQLiteCommand command = new SQLiteCommand(queryString, connection);
                connection.Open();
            
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tablero = new Tablero();
                        tablero.Id = Convert.ToInt32(reader["id"]);
                        tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                        tablero.Nombre = reader["nombre"].ToString();
                        tablero.Descripcion = reader["descripcion"].ToString();
                        tableros.Add(tablero);
                    }
                }
                connection.Close();
            }
            return tableros;
        }
        public Tablero Create(Tablero newTablero){
            var query = $"INSERT INTO Tablero (id, id_usuario_propietario,nombre,descripcion) VALUES (@Id,@IdPropietario,@name,@descrip)";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@Id", newTablero.Id));
                command.Parameters.Add(new SQLiteParameter("@IdPropietario", newTablero.IdUsuarioPropietario));
                command.Parameters.Add(new SQLiteParameter("@name", newTablero.Nombre));
                command.Parameters.Add(new SQLiteParameter("@descrip", newTablero.Descripcion));
                command.ExecuteNonQuery();

                connection.Close();   
            }
            return(newTablero);
        }

        public Tablero GetById(int Id){
            var tablero = new Tablero();

            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Tablero WHERE id = @Id";
            command.Parameters.Add(new SQLiteParameter("@Id", Id));
            connection.Open();
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tablero.Nombre= reader["nombre"].ToString();
                    tablero.Descripcion= reader["descripcion"].ToString();
                }
            }
            connection.Close();
            return(tablero);
        }
        public void Update(Tablero tablero){
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            using (connection)
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                using (command)
                {
                    command.CommandText = "UPDATE Tablero SET nombre = @name, id_usuario_propietario = @propietario, descripcion = @descr WHERE id = @Id;";
                    command.Parameters.AddWithValue("@Id", tablero.Id);
                    command.Parameters.AddWithValue("@name", tablero.Nombre);
                    command.Parameters.AddWithValue("@propietario", tablero.IdUsuarioPropietario);
                    command.Parameters.AddWithValue("@descr", tablero.Descripcion);
                    command.ExecuteNonQuery();
                }
                connection.Close();   
            }
        }
        public void Remove(int Id){
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            using (connection)
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                using (command)
                {
                    command.CommandText = "DELETE FROM Tablero WHERE id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();      
                }
                connection.Close();
            }
        }

        public List<Tablero> GetListaTableros(int Id){
            List<Tablero> tableros = new List<Tablero>();
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            using(connection)
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                using(command)
                {
                    command.CommandText = "SELECT * FROM Tablero WHERE id_usuario_propietario = @Id";
                    command.Parameters.Add(new SQLiteParameter("@Id", Id));
                    command.ExecuteNonQuery(); 
                    var reader = command.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            var tablero = new Tablero();
                            tablero.Id = Convert.ToInt32(reader["id"]);
                            tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                            tablero.Nombre = reader["nombre"].ToString();
                            tablero.Descripcion = reader["descripcion"].ToString();
                            tableros.Add(tablero);
                        }
                    }
                }
                connection.Close();
            }
            return(tableros);
        }
    }
}