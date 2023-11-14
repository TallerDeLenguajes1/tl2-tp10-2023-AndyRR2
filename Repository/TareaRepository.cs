using System.Data.SQLite;

using Tp10.Models;

namespace EspacioTareaRepository{
    public class TareasRepository : ITareasRepository
    {
        private string cadenaConexion = "Data Source=C:/Taller_2/tl2-tp09-2023-AndyRR2/Tp8/kamban.db;Cache=Shared";
        public Tarea Create(Tarea newTarea){
            var query = $"INSERT INTO Tarea (id,id_tablero,nombre,estado,descripcion,color,id_usuario_asignado) VALUES (@Id,@IdTab,@name,@Estado,@Descr,@Colo,@IdUsuario)";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@Id", newTarea.Id));
                command.Parameters.Add(new SQLiteParameter("@IdTab", newTarea.IdTablero));
                command.Parameters.Add(new SQLiteParameter("@IdUsuario", newTarea.IdUsuarioAsignado));
                command.Parameters.Add(new SQLiteParameter("@name", newTarea.Nombre));
                command.Parameters.Add(new SQLiteParameter("@Estado", newTarea.Estado));
                command.Parameters.Add(new SQLiteParameter("@Descr", newTarea.Descripcion));
                command.Parameters.Add(new SQLiteParameter("@Colo", newTarea.Color));
                command.ExecuteNonQuery();

                connection.Close();   
            }
            return(newTarea);
        }

        public List<Tarea> GetAll(){
            var queryString = @"SELECT * FROM Tarea;";
            List<Tarea> tareas = new List<Tarea>();
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                SQLiteCommand command = new SQLiteCommand(queryString, connection);
                connection.Open();
            
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tarea = new Tarea();
                        tarea.Id = Convert.ToInt32(reader["id"]);
                        tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                        tarea.IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                        tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                        tarea.Nombre = reader["nombre"].ToString();
                        tarea.Descripcion = reader["descripcion"].ToString();
                        tarea.Color = reader["color"].ToString();
                        tareas.Add(tarea);
                    }
                }
                connection.Close();
            }
            return tareas;
        }

        public Tarea GetById(int Id){
            
            var tarea = new Tarea();

            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Tarea WHERE id = @Id";
            command.Parameters.Add(new SQLiteParameter("@Id", Id));
            connection.Open();
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tarea.Id = Convert.ToInt32(reader["id"]);
                    tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                    tarea.IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Nombre = reader["nombre"].ToString();
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                }
            }
            connection.Close();
            return(tarea);
        }
        public List<Tarea> GetTareasDeTablero(int Id){
            List<Tarea> tareas = new List<Tarea>();
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            using(connection)
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                using(command)
                {
                    command.CommandText = "SELECT * FROM Tarea WHERE id_tablero = @Id";
                    command.Parameters.Add(new SQLiteParameter("@Id", Id));
                    command.ExecuteNonQuery(); 
                    var reader = command.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            var tarea = new Tarea();
                            tarea.Id = Convert.ToInt32(reader["id"]);
                            tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                            tarea.IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                            tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                            tarea.Nombre = reader["nombre"].ToString();
                            tarea.Descripcion = reader["descripcion"].ToString();
                            tarea.Color = reader["color"].ToString();
                            tareas.Add(tarea);
                        }
                    }
                }
                connection.Close();
            }
            return(tareas);
        }

        public List<Tarea> GetTareasDeUsuario(int Id){
            List<Tarea> tareas = new List<Tarea>();
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            using(connection)
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                using(command)
                {
                    command.CommandText = "SELECT * FROM Tarea WHERE id_usuario_asignado = @Id";
                    command.Parameters.Add(new SQLiteParameter("@Id", Id));
                    command.ExecuteNonQuery(); 
                    var reader = command.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            var tarea = new Tarea();
                            tarea.Id = Convert.ToInt32(reader["id"]);
                            tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                            tarea.IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                            tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                            tarea.Nombre = reader["nombre"].ToString();
                            tarea.Descripcion = reader["descripcion"].ToString();
                            tarea.Color = reader["color"].ToString();
                            tareas.Add(tarea);
                        }
                    }
                }
                connection.Close();
            }
            return(tareas);
        }

        public int ContarTareasEstado(int estado){
            int cantidad = 0;
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            using(connection){
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                using(command){
                    command.CommandText = "SELECT COUNT(*) FROM Tarea WHERE estado = @Estado";
                    command.Parameters.Add(new SQLiteParameter("@Estado", estado));
                    command.ExecuteNonQuery(); 
                    var reader = command.ExecuteReader();
                    using(reader){
                        if (reader.Read())
                        {
                            cantidad = Convert.ToInt32(reader[0]);
                        }
                    }
                }
                connection.Close();
            }
            return(cantidad);
        }

        public void Remove(int Id){
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            using (connection)
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                using (command)
                {
                    command.CommandText = "DELETE FROM Tarea WHERE id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();      
                }
                connection.Close();
            }
        }

        public void Update(Tarea tarea){
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            using (connection)
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                using (command)
                {
                    command.CommandText = "UPDATE Tarea SET nombre = @name, id_usuario_asignado = @IdUsuario, descripcion = @Descr, id_tablero = @IdTab, estado = @Estado, color = @Colo WHERE id = @Id;";
                    command.Parameters.AddWithValue("@Id", tarea.Id);
                    command.Parameters.AddWithValue("@IdTab", tarea.IdTablero);
                    command.Parameters.AddWithValue("@IdUsuario", tarea.IdUsuarioAsignado);
                    command.Parameters.AddWithValue("@name", tarea.Nombre);
                    command.Parameters.AddWithValue("@Estado", tarea.Estado);
                    command.Parameters.AddWithValue("@Descr", tarea.Descripcion);
                    command.Parameters.AddWithValue("@Colo", tarea.Color);
                    command.ExecuteNonQuery();
                }
                connection.Close();   
            }
        }

        
    }
}