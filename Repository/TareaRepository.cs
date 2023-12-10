namespace EspacioTareaRepository;

using System.Data.SQLite;
using Tp11.Models;

public class TareaRepository : ITareaRepository{
    private readonly string direccionBD = "Data Source = DataBase/kamban.db;Cache=Shared";

    public void Create(Tarea newTarea){
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

        string queryC = $"INSERT INTO Tarea (id,id_tablero,nombre,estado,descripcion,color,id_usuario_asignado) VALUES (@ID,@IDTAB,@NAME,@ESTADO,@DESCRIPCION,@COLOR,@IDUSU)";
        SQLiteParameter parameterId = new SQLiteParameter("@ID",newTarea.Id);
        SQLiteParameter parameterIdTab = new SQLiteParameter("@IDTAB",newTarea.IdTablero);
        SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",newTarea.Nombre);
        SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",newTarea.Estado);
        SQLiteParameter parameterDescripcion = new SQLiteParameter("@DESCRIPCION",newTarea.Descripcion);
        SQLiteParameter parameterColor = new SQLiteParameter("@COLOR",newTarea.Color);
        SQLiteParameter parameterIdUsu = new SQLiteParameter("@IDUSU",newTarea.IdUsuarioAsignado);
        
        using (connectionC)
        {
            connectionC.Open();
            var commandC = new SQLiteCommand(queryC, connectionC);
            commandC.Parameters.Add(parameterId);
            commandC.Parameters.Add(parameterIdTab);
            commandC.Parameters.Add(parameterNombre);
            commandC.Parameters.Add(parameterEstado);
            commandC.Parameters.Add(parameterDescripcion);
            commandC.Parameters.Add(parameterColor);
            commandC.Parameters.Add(parameterIdUsu);
            
            commandC.ExecuteNonQuery();
            connectionC.Close();   
        }
        if (newTarea == null){
            throw new Exception("La Tarea no se creo correctamente.");
        }
    }
    public void Update(Tarea tareaAEditar){
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

        string queryC = "UPDATE Tarea SET nombre = @NAME, id_usuario_asignado = @IDUSU, descripcion = @DESCRIPCION, id_tablero = @IDTAB, estado = @ESTADO, color = @COLOR WHERE id = @ID;";
        SQLiteParameter parameterId = new SQLiteParameter("@ID",tareaAEditar.Id);
        SQLiteParameter parameterIdTab = new SQLiteParameter("@IDTAB",tareaAEditar.IdTablero);
        SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",tareaAEditar.Nombre);
        SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",tareaAEditar.Estado);
        SQLiteParameter parameterDescripcion = new SQLiteParameter("@DESCRIPCION",tareaAEditar.Descripcion);
        SQLiteParameter parameterColor = new SQLiteParameter("@COLOR",tareaAEditar.Color);
        SQLiteParameter parameterIdUsu = new SQLiteParameter("@IDUSU",tareaAEditar.IdUsuarioAsignado);
        using (connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterId);
            commandC.Parameters.Add(parameterIdTab);
            commandC.Parameters.Add(parameterNombre);
            commandC.Parameters.Add(parameterEstado);
            commandC.Parameters.Add(parameterDescripcion);
            commandC.Parameters.Add(parameterColor);
            commandC.Parameters.Add(parameterIdUsu);

            int rowsAffected = commandC.ExecuteNonQuery();
            connectionC.Close();
            if (rowsAffected == 0){
                throw new Exception("No se encontró ninguna tarea con el ID proporcionado.");
            }   
        }
    }
    public Tarea GetById(int? idUsuario){
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
        Tarea tareaSelec = new Tarea();
        string queryC = "SELECT * FROM Tarea WHERE id = @ID";
        SQLiteParameter parameterId = new SQLiteParameter("@ID", idUsuario);
        
        using (connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterId);

            SQLiteDataReader readerC = commandC.ExecuteReader();
            using(readerC)
            {
                while (readerC.Read())
                {
                    tareaSelec.Id = Convert.ToInt32(readerC["id"]);
                    tareaSelec.IdTablero = Convert.ToInt32(readerC["id_tablero"]);
                    tareaSelec.Nombre = Convert.ToString(readerC["nombre"]);
                    tareaSelec.Estado = (EstadoTarea)Convert.ToInt32(readerC["estado"]);
                    tareaSelec.Descripcion = Convert.ToString(readerC["descripcion"]);
                    tareaSelec.Color = Convert.ToString(readerC["color"]);
                    tareaSelec.IdUsuarioAsignado = Convert.ToInt32(readerC["id_usuario_asignado"]);
                }
            }
            connectionC.Close();
        }
        if (tareaSelec == null){
            throw new Exception("La Tarea no esta creada.");
        }
        return(tareaSelec);
    }
    public List<Tarea> GetTareasDeUsuario(int? Id){
        List<Tarea> tareas = new List<Tarea>();
        SQLiteConnection connection = new SQLiteConnection(direccionBD);
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
        if (tareas == null){
            throw new Exception("El usuario proporcionado no tiene tareas.");
        }
        return(tareas);
    }
    public List<Tarea> GetTareasDeTablero(int? Id){
        List<Tarea> tareas = new List<Tarea>();
        SQLiteConnection connection = new SQLiteConnection(direccionBD);
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
        if (tareas == null){
            throw new Exception("El tablero proporcionado no tiene tareas.");
        } 
        return(tareas);
    }
    public void Remove(int? idUsuario){
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

        string queryC = "DELETE FROM Tarea WHERE id = @ID";
        SQLiteParameter parameterId = new SQLiteParameter("@ID", idUsuario);
        
        using (connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterId);

            int rowsAffected = commandC.ExecuteNonQuery();
            connectionC.Close();
            if (rowsAffected == 0){
                throw new Exception("No se encontró ninguna tarea con el ID proporcionado.");
            }
        }
    }
    public void AsignarUsuario(Tarea tareaModificada){
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

        string queryC = "UPDATE Tarea SET id_usuario_asignado = @IDUSU WHERE id = @ID;";
        SQLiteParameter parameterId = new SQLiteParameter("@ID",tareaModificada.Id);
        SQLiteParameter parameterIdUsu = new SQLiteParameter("@IDUSU",tareaModificada.IdUsuarioAsignado);
        
        using (connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterId);
            commandC.Parameters.Add(parameterIdUsu);

            int rowsAffected = commandC.ExecuteNonQuery();
            connectionC.Close();
            if (rowsAffected == 0){
                throw new Exception("No se encontró ninguna tarea con el ID proporcionado.");
            } 
        }
    }
    public List<Tarea> GetAll(){
        List<Tarea> tareas = new List<Tarea>();
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

        string queryC = @"SELECT * FROM Tarea;";

        using (connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC, connectionC);
            
            SQLiteDataReader readerC = commandC.ExecuteReader();
            using(readerC)
            {
                while (readerC.Read())
                {
                    var newTarea = new Tarea();
                    newTarea.Id = Convert.ToInt32(readerC["id"]);
                    newTarea.IdTablero = Convert.ToInt32(readerC["id_tablero"]);
                    newTarea.Nombre = Convert.ToString(readerC["nombre"]);
                    newTarea.Estado = (EstadoTarea)Convert.ToInt32(readerC["estado"]);
                    newTarea.Descripcion = Convert.ToString(readerC["descripcion"]);
                    newTarea.Color = Convert.ToString(readerC["color"]);
                    newTarea.IdUsuarioAsignado = Convert.ToInt32(readerC["id_usuario_asignado"]);
                    tareas.Add(newTarea);
                }
            }
            connectionC.Close();
        }
        if (tareas == null){
            throw new Exception("Lista de tareas no encontrada.");
        }
        return tareas;
    }
    /*public int ContarTareasEstado(int estado){
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
    }*/
}
