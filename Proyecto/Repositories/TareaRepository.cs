using System.Data.SQLite;

using Proyecto.Models;

namespace Proyecto.Repositories{
    public class TareaRepository: ITareaRepository{
        private readonly string direccionBD;
        public TareaRepository(string cadenaDeConexion)
        {
            direccionBD = cadenaDeConexion;
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
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_tablero"))){
                            newTarea.IdTablero = Convert.ToInt32(readerC["id_tablero"]);
                        }else{
                            newTarea.IdTablero = null;
                        }
                        newTarea.Nombre = Convert.ToString(readerC["nombre"]);
                        newTarea.EstadoTarea = (EstadoTarea)Convert.ToInt32(readerC["estado"]);
                        newTarea.Descripcion = Convert.ToString(readerC["descripcion"]);
                        newTarea.Color = (Color)Convert.ToInt32(readerC["color"]);
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_propietario"))){
                            newTarea.IdUsuarioPropietario = Convert.ToInt32(readerC["id_usuario_propietario"]);
                        }else{
                            newTarea.IdUsuarioPropietario = null;
                        }
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_asignado"))){
                            newTarea.IdUsuarioAsignado = Convert.ToInt32(readerC["id_usuario_asignado"]);
                        }else{
                            newTarea.IdUsuarioAsignado = null;
                        }
                        tareas.Add(newTarea);
                    }
                }
                connectionC.Close();
            }
            if (tareas == null){
                throw new Exception("No se encontraron tareas en la base de datos.");
            }
            return tareas;
        }
        public Tarea GetById(int? idTarea){
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            Tarea tareaSelec = new Tarea();
            string queryC = "SELECT * FROM Tarea WHERE id = @ID";
            SQLiteParameter parameterId = new SQLiteParameter("@ID", idTarea);
            
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
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_tablero"))){
                            tareaSelec.IdTablero = Convert.ToInt32(readerC["id_tablero"]);
                        }else{
                            tareaSelec.IdTablero = null;
                        }
                        tareaSelec.Nombre = Convert.ToString(readerC["nombre"]);
                        tareaSelec.EstadoTarea = (EstadoTarea)Convert.ToInt32(readerC["estado"]);
                        tareaSelec.Descripcion = Convert.ToString(readerC["descripcion"]);
                        tareaSelec.Color = (Color)Convert.ToInt32(readerC["color"]);
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_propietario"))){
                            tareaSelec.IdUsuarioPropietario = Convert.ToInt32(readerC["id_usuario_propietario"]);
                        }else{
                            tareaSelec.IdUsuarioPropietario = null;
                        }
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_asignado"))){
                            tareaSelec.IdUsuarioAsignado = Convert.ToInt32(readerC["id_usuario_asignado"]);
                        }else{
                            tareaSelec.IdUsuarioAsignado = null;
                        }
                    }
                }
                connectionC.Close();
            }
            if (tareaSelec == null){
                throw new Exception("No se encontró ninguna tarea con el ID proporcionado.");
            }
            return(tareaSelec);
        }
        public void Create(Tarea newTarea){
            if (TaskExists(newTarea.Nombre))
            {
                throw new Exception("La Tarea ya existe.");
            }
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = $"INSERT INTO Tarea (id_tablero,nombre,estado,descripcion,color,id_usuario_asignado,id_usuario_propietario) VALUES (@IDTAB,@NAME,@ESTADO,@DESCRIPCION,@COLOR,@IDUSUA,@IDUSUP)";
            SQLiteParameter parameterIdTab = new SQLiteParameter("@IDTAB",newTarea.IdTablero);
            SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",newTarea.Nombre);
            SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",newTarea.EstadoTarea);
            SQLiteParameter parameterDescripcion = new SQLiteParameter("@DESCRIPCION",newTarea.Descripcion);
            SQLiteParameter parameterColor = new SQLiteParameter("@COLOR",newTarea.Color);
            SQLiteParameter parameterIdUsuA = new SQLiteParameter("@IDUSUA",newTarea.IdUsuarioAsignado);
            SQLiteParameter parameterIdUsuP = new SQLiteParameter("@IDUSUP",newTarea.IdUsuarioPropietario);
            
            using (connectionC)
            {
                connectionC.Open();
                var commandC = new SQLiteCommand(queryC, connectionC);
                commandC.Parameters.Add(parameterIdTab);
                commandC.Parameters.Add(parameterNombre);
                commandC.Parameters.Add(parameterEstado);
                commandC.Parameters.Add(parameterDescripcion);
                commandC.Parameters.Add(parameterColor);
                commandC.Parameters.Add(parameterIdUsuA);
                commandC.Parameters.Add(parameterIdUsuP);
                
                commandC.ExecuteNonQuery();
                connectionC.Close();   
            }
            if (newTarea == null){
                throw new Exception("La Tarea no se creo correctamente.");
            }
        }
        public void Update(Tarea tareaAEditar){
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "UPDATE Tarea SET nombre = @NAME, descripcion = @DESCRIPCION, id_tablero = @IDTAB, estado = @ESTADO, color = @COLOR WHERE id = @ID;";
            SQLiteParameter parameterId = new SQLiteParameter("@ID",tareaAEditar.Id);
            SQLiteParameter parameterIdTab = new SQLiteParameter("@IDTAB",tareaAEditar.IdTablero);
            SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",tareaAEditar.Nombre);
            SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",tareaAEditar.EstadoTarea);
            SQLiteParameter parameterDescripcion = new SQLiteParameter("@DESCRIPCION",tareaAEditar.Descripcion);
            SQLiteParameter parameterColor = new SQLiteParameter("@COLOR",tareaAEditar.Color);
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

                int rowsAffected = commandC.ExecuteNonQuery();
                connectionC.Close();
                if (rowsAffected == 0){
                    throw new Exception("No se encontró ninguna tarea con el ID proporcionado.");
                }   
            }
        }
        public void Remove(int? idTarea){
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "DELETE FROM Tarea WHERE id = @ID";
            SQLiteParameter parameterId = new SQLiteParameter("@ID", idTarea);
            
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
        public void Assign(int? idTarea, int? idUsuario){
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "UPDATE Tarea SET id_usuario_asignado = @IDUSU WHERE id = @ID;";
            SQLiteParameter parameterId = new SQLiteParameter("@ID",idTarea);
            SQLiteParameter parameterIdUsu = new SQLiteParameter("@IDUSU",idUsuario);
            
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
        public void Disable(int? idTarea, int? idTablero){
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            
            string queryC = "UPDATE Tarea SET estado = @ESTADO, id_tablero = @IDTAB, id_usuario_asignado = NULL, id_usuario_propietario = NULL WHERE id = @ID";
            SQLiteParameter parameterId = new SQLiteParameter("@ID",idTarea);
            SQLiteParameter parameterIdTab = new SQLiteParameter("@IDTAB",idTablero);
            SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",6);

            using (connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterId);
                commandC.Parameters.Add(parameterIdTab);
                commandC.Parameters.Add(parameterEstado);

                int rowAffected =  commandC.ExecuteNonQuery();
                connectionC.Close();
                if (rowAffected == 0){
                    throw new Exception("No se encontró ninguna tarea con el ID proporcionado.");
                }
            }
        }
        public List<Tarea> GetByOwnerBoard(int? idTablero){
            List<Tarea> tareas = new List<Tarea>();
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            
            string queryC = "SELECT * FROM Tarea WHERE id_tablero = @IDTAB";
            SQLiteParameter parameterIdTab = new SQLiteParameter("@IDTAB",idTablero);

            using(connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterIdTab);

                SQLiteDataReader readerC = commandC.ExecuteReader();
                using (readerC)
                {
                    while (readerC.Read())
                    {
                        Tarea newTarea = new Tarea();
                        newTarea.Id = Convert.ToInt32(readerC["id"]);
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_tablero"))){
                            newTarea.IdTablero = Convert.ToInt32(readerC["id_tablero"]);
                        }else{
                            newTarea.IdTablero = null;
                        }
                        newTarea.IdTablero = Convert.ToInt32(readerC["id_tablero"]);
                        newTarea.Nombre = Convert.ToString(readerC["nombre"]);
                        newTarea.EstadoTarea = (EstadoTarea)Convert.ToInt32(readerC["estado"]);
                        newTarea.Descripcion = Convert.ToString(readerC["descripcion"]);
                        newTarea.Color = (Color)Convert.ToInt32(readerC["color"]);
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_propietario"))){
                            newTarea.IdUsuarioPropietario = Convert.ToInt32(readerC["id_usuario_propietario"]);
                        }else{
                            newTarea.IdUsuarioPropietario = null;
                        }
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_asignado"))){
                            newTarea.IdUsuarioAsignado = Convert.ToInt32(readerC["id_usuario_asignado"]);
                        }else{
                            newTarea.IdUsuarioAsignado = null;
                        }
                        tareas.Add(newTarea);
                    }
                }
                connectionC.Close();           
            }
            if (tareas == null){
                throw new Exception("El Tablero proporcionado no tiene tareas.");
            }
            return(tareas);
        }
        public List<Tarea> GetByOwnerUser(int? idUsuario){
            List<Tarea> tareas = new List<Tarea>();
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            
            string queryC = "SELECT * FROM Tarea WHERE id_usuario_propietario = @IDUSU OR id_usuario_asignado = @IDUSU";
            SQLiteParameter parameterIdTab = new SQLiteParameter("@IDUSU",idUsuario);

            using(connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterIdTab);

                SQLiteDataReader readerC = commandC.ExecuteReader();
                using (readerC)
                {
                    while (readerC.Read())
                    {
                        Tarea newTarea = new Tarea();
                        newTarea.Id = Convert.ToInt32(readerC["id"]);
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_tablero"))){
                            newTarea.IdTablero = Convert.ToInt32(readerC["id_tablero"]);
                        }else{
                            newTarea.IdTablero = null;
                        }
                        newTarea.Nombre = Convert.ToString(readerC["nombre"]);
                        newTarea.EstadoTarea = (EstadoTarea)Convert.ToInt32(readerC["estado"]);
                        newTarea.Descripcion = Convert.ToString(readerC["descripcion"]);
                        newTarea.Color = (Color)Convert.ToInt32(readerC["color"]);
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_propietario"))){
                            newTarea.IdUsuarioPropietario = Convert.ToInt32(readerC["id_usuario_propietario"]);
                        }else{
                            newTarea.IdUsuarioPropietario = null;
                        }
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_asignado"))){
                            newTarea.IdUsuarioAsignado = Convert.ToInt32(readerC["id_usuario_asignado"]);
                        }else{
                            newTarea.IdUsuarioAsignado = null;
                        }
                        tareas.Add(newTarea);
                    }
                }
                connectionC.Close();           
            }
            if (tareas == null){
                throw new Exception("El Usuario proporcionado no tiene tareas.");
            }
            return(tareas);
        }
        public bool ChechAsignedTask(int? idTablero, int? idUsuario){
            bool validacion = false;
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "SELECT * FROM Tarea WHERE id_usuario_asignado = @IDASIGN AND id_tablero = @IDTAB";
            SQLiteParameter parameterIdAsign = new SQLiteParameter("@IDASIGN", idUsuario);
            SQLiteParameter parameterIdTab = new SQLiteParameter("@IDTAB", idTablero);

            using (connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterIdAsign);
                commandC.Parameters.Add(parameterIdTab);

                SQLiteDataReader readerC = commandC.ExecuteReader();
                using (readerC)
                {
                    while (readerC.Read())
                    {
                        validacion = true;
                    }
                }
                connectionC.Close();
            }
            if (validacion == false)
            {
                throw new Exception("No se encontraron tareas asignadas al tablero proporcionado en la base de datos.");
            }
            return validacion;
        }
        public bool TaskExists(string? nombreTarea){
            bool validacion=false;
            string? Nombre=null;
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "SELECT * FROM Tarea WHERE nombre = @NAME";
            SQLiteParameter parameterName = new SQLiteParameter("@NAME",nombreTarea);

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
                        Nombre = Convert.ToString(readerC["nombre"]);
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