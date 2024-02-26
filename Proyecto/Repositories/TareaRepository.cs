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

            string queryC = @"SELECT Tarea.id AS TareaId, Tarea.id_tablero, Tarea.nombre, Tarea.estado, Tarea.descripcion, Tarea.color, Tarea.id_usuario_asignado, Tarea.id_usuario_propietario, 
                            Tablero.nombre_tablero AS nombre_Tablero, 
                            UsuarioAsignado.nombre_de_usuario AS nombre_asignado, 
                            UsuarioPropietario.nombre_de_usuario AS nombre_propietario
                            FROM Tarea
                            LEFT JOIN Tablero ON Tablero.id = Tarea.id_tablero
                            LEFT JOIN Usuario AS UsuarioAsignado ON UsuarioAsignado.id = Tarea.id_usuario_asignado
                            LEFT JOIN Usuario AS UsuarioPropietario ON UsuarioPropietario.id = Tarea.id_usuario_propietario;";

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
                        newTarea.Id = Convert.ToInt32(readerC["TareaId"]);
                        newTarea.Nombre = Convert.ToString(readerC["nombre"]);
                        newTarea.EstadoTarea = (EstadoTarea)Convert.ToInt32(readerC["estado"]);
                        newTarea.Descripcion = Convert.ToString(readerC["descripcion"]);
                        newTarea.Color = (Color)Convert.ToInt32(readerC["color"]);
                        newTarea.TableroPropio = new Tablero();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_tablero"))){
                            newTarea.TableroPropio.Id = Convert.ToInt32(readerC["id_tablero"]);
                            newTarea.TableroPropio.Nombre = Convert.ToString(readerC["nombre_Tablero"]);
                        }
                        newTarea.Asignado = new Usuario();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_asignado"))){
                            newTarea.Asignado.Id = Convert.ToInt32(readerC["id_usuario_asignado"]);
                            newTarea.Asignado.Nombre = Convert.ToString(readerC["nombre_asignado"]);
                        }
                        newTarea.Propietario = new Usuario();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_propietario"))){
                            newTarea.Propietario.Id = Convert.ToInt32(readerC["id_usuario_propietario"]);
                            newTarea.Propietario.Nombre = Convert.ToString(readerC["nombre_propietario"]);
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
        public List<Tarea> GetAllByOwnerBoard(int? idTablero){
            List<Tarea> tareas = new List<Tarea>();
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            
            string queryC = @"SELECT Tarea.id AS TareaId, Tarea.id_tablero, Tarea.nombre, Tarea.estado, Tarea.descripcion, Tarea.color, Tarea.id_usuario_asignado, Tarea.id_usuario_propietario, 
                            Tablero.nombre_tablero AS nombre_Tablero, 
                            UsuarioAsignado.nombre_de_usuario AS nombre_asignado, 
                            UsuarioPropietario.nombre_de_usuario AS nombre_propietario
                            FROM Tarea
                            INNER JOIN Tablero ON Tablero.id = Tarea.id_tablero
                            LEFT JOIN Usuario AS UsuarioAsignado ON UsuarioAsignado.id = Tarea.id_usuario_asignado
                            LEFT JOIN Usuario AS UsuarioPropietario ON UsuarioPropietario.id = Tarea.id_usuario_propietario
                            WHERE Tarea.id_tablero = @IDTAB;"; 
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
                        var newTarea = new Tarea();
                        newTarea.Id = Convert.ToInt32(readerC["TareaId"]);
                        newTarea.Nombre = Convert.ToString(readerC["nombre"]);
                        newTarea.EstadoTarea = (EstadoTarea)Convert.ToInt32(readerC["estado"]);
                        newTarea.Descripcion = Convert.ToString(readerC["descripcion"]);
                        newTarea.Color = (Color)Convert.ToInt32(readerC["color"]);
                        newTarea.TableroPropio = new Tablero();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_tablero"))){
                            newTarea.TableroPropio.Id = Convert.ToInt32(readerC["id_tablero"]);
                            newTarea.TableroPropio.Nombre = Convert.ToString(readerC["nombre_Tablero"]);
                        }
                        newTarea.Asignado = new Usuario();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_asignado"))){
                            newTarea.Asignado.Id = Convert.ToInt32(readerC["id_usuario_asignado"]);
                            newTarea.Asignado.Nombre = Convert.ToString(readerC["nombre_asignado"]);
                        }
                        newTarea.Propietario = new Usuario();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_propietario"))){
                            newTarea.Propietario.Id = Convert.ToInt32(readerC["id_usuario_propietario"]);
                            newTarea.Propietario.Nombre = Convert.ToString(readerC["nombre_propietario"]);
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
        public Tarea GetById(int? idTarea){
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            Tarea newTarea = new Tarea();
            string queryC = @"SELECT Tarea.id AS TareaId, Tarea.id_tablero, Tarea.nombre, Tarea.estado, Tarea.descripcion, Tarea.color, Tarea.id_usuario_asignado, Tarea.id_usuario_propietario, 
                            Tablero.nombre_tablero AS nombre_Tablero, 
                            UsuarioAsignado.nombre_de_usuario AS nombre_asignado, 
                            UsuarioPropietario.nombre_de_usuario AS nombre_propietario
                            FROM Tarea
                            LEFT JOIN Tablero ON Tablero.id = Tarea.id_tablero
                            LEFT JOIN Usuario AS UsuarioAsignado ON UsuarioAsignado.id = Tarea.id_usuario_asignado
                            LEFT JOIN Usuario AS UsuarioPropietario ON UsuarioPropietario.id = Tarea.id_usuario_propietario
                            WHERE Tarea.id = @ID;"; 
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
                        newTarea.Id = Convert.ToInt32(readerC["TareaId"]);
                        newTarea.Nombre = Convert.ToString(readerC["nombre"]);
                        newTarea.EstadoTarea = (EstadoTarea)Convert.ToInt32(readerC["estado"]);
                        newTarea.Descripcion = Convert.ToString(readerC["descripcion"]);
                        newTarea.Color = (Proyecto.Models.Color)Convert.ToInt32(readerC["color"]);
                        newTarea.TableroPropio = new Tablero();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_tablero"))){
                            newTarea.TableroPropio.Id = Convert.ToInt32(readerC["id_tablero"]);
                            newTarea.TableroPropio.Nombre = Convert.ToString(readerC["nombre_Tablero"]);
                        }
                        newTarea.Asignado = new Usuario();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_asignado"))){
                            newTarea.Asignado.Id = Convert.ToInt32(readerC["id_usuario_asignado"]);
                            newTarea.Asignado.Nombre = Convert.ToString(readerC["nombre_asignado"]);
                        }
                        newTarea.Propietario = new Usuario();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_propietario"))){
                            newTarea.Propietario.Id = Convert.ToInt32(readerC["id_usuario_propietario"]);
                            newTarea.Propietario.Nombre = Convert.ToString(readerC["nombre_propietario"]);
                        }
                    }
                }
                connectionC.Close();
            }
            if (newTarea == null){
                throw new Exception("No se encontró ninguna tarea con el ID proporcionado.");
            }
            return(newTarea);
        }
        public void Create(Tarea newTarea){
            if (TaskExists(newTarea.Nombre))
            {
                throw new Exception("La Tarea ya existe.");
            }
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = $"INSERT INTO Tarea (id_tablero,nombre,estado,descripcion,color,id_usuario_asignado,id_usuario_propietario) VALUES (@IDTAB,@NAME,@ESTADO,@DESCRIPCION,@COLOR,@IDUSUA,@IDUSUP)";
            SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",newTarea.Nombre);
            SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",newTarea.EstadoTarea);
            SQLiteParameter parameterDescripcion = new SQLiteParameter("@DESCRIPCION",newTarea.Descripcion);
            SQLiteParameter parameterColor = new SQLiteParameter("@COLOR",newTarea.Color);
            SQLiteParameter parameterIdUsuA = new SQLiteParameter("@IDUSUA",newTarea.Asignado.Id);
            SQLiteParameter parameterIdTab = new SQLiteParameter("@IDTAB",newTarea.TableroPropio.Id);
            SQLiteParameter parameterIdUsuP = new SQLiteParameter("@IDUSUP",newTarea.Propietario.Id);
            
            using (connectionC)
            {
                connectionC.Open();
                var commandC = new SQLiteCommand(queryC, connectionC);
                commandC.Parameters.Add(parameterNombre);
                commandC.Parameters.Add(parameterEstado);
                commandC.Parameters.Add(parameterDescripcion);
                commandC.Parameters.Add(parameterColor);
                commandC.Parameters.Add(parameterIdTab);
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

            string queryC = "UPDATE Tarea SET nombre = @NAME, descripcion = @DESCRIPCION, id_tablero = @IDTAB, estado = @ESTADO, color = @COLOR, id_usuario_propietario = @IDUSUP WHERE id = @ID;";
            SQLiteParameter parameterId = new SQLiteParameter("@ID",tareaAEditar.Id);
            SQLiteParameter parameterIdTab = new SQLiteParameter("@IDTAB",tareaAEditar.TableroPropio.Id);
            SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",tareaAEditar.Nombre);
            SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",tareaAEditar.EstadoTarea);
            SQLiteParameter parameterDescripcion = new SQLiteParameter("@DESCRIPCION",tareaAEditar.Descripcion);
            SQLiteParameter parameterColor = new SQLiteParameter("@COLOR",tareaAEditar.Color);
            SQLiteParameter parameterIdUsuP = new SQLiteParameter("@IDUSUP",tareaAEditar.Propietario.Id);
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
                commandC.Parameters.Add(parameterIdUsuP);

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
        public void ChangeStatus(Tarea tarea){
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "UPDATE Tarea SET estado = @ESTADO WHERE id = @ID;";
            SQLiteParameter parameterId = new SQLiteParameter("@ID",tarea.Id);
            SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",tarea.EstadoTarea);
            
            using (connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterId);
                commandC.Parameters.Add(parameterEstado);

                int rowsAffected = commandC.ExecuteNonQuery();
                connectionC.Close();
                if (rowsAffected == 0){
                    throw new Exception("No se encontró ninguna tarea con el ID proporcionado.");
                }   
            }
        }
        public void DisableByDeletedBoard(int? idTarea){
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            
            string queryC = "UPDATE Tarea SET estado = @ESTADO, id_tablero = NULL WHERE id = @ID;";
            SQLiteParameter parameterId = new SQLiteParameter("@ID",idTarea);
            SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",6);

            using (connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterId);
                commandC.Parameters.Add(parameterEstado);

                int rowAffected =  commandC.ExecuteNonQuery();
                connectionC.Close();
                if (rowAffected == 0){
                    throw new Exception("No se encontró ninguna tarea con el ID proporcionado.");
                }
            }
        }
        public void DisableByDeletedUser(int? idUsuario){
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            
            string queryAsignado = @"UPDATE Tarea SET id_usuario_asignado = NULL, estado = @ESTADO
                                   WHERE id_usuario_asignado = @ID;";

            string queryPropietario = @"UPDATE Tarea SET id_usuario_propietario = NULL, estado = @ESTADO
                                      WHERE id_usuario_propietario = @ID;";
            
            SQLiteParameter parameterId = new SQLiteParameter("@ID",idUsuario);
            SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",6);

            using (connectionC)
            {
                connectionC.Open();
                
                SQLiteCommand commandA = new SQLiteCommand(queryAsignado,connectionC);
                commandA.Parameters.Add(parameterId);
                commandA.Parameters.Add(parameterEstado);
                
                SQLiteCommand commandP = new SQLiteCommand(queryPropietario,connectionC);
                commandP.Parameters.Add(parameterId);
                commandP.Parameters.Add(parameterEstado);

                int rowAffectedA =  commandA.ExecuteNonQuery();
                int rowAffectedP =  commandP.ExecuteNonQuery();
                connectionC.Close();
                if (rowAffectedA == 0 && rowAffectedP == 0){
                    throw new Exception("No se encontró ninguna tarea para el usuario proporcionado.");
                }
            }
        }
        
        public List<Tarea> GetAllByOwnerUser(int? idUsuario){
            List<Tarea> tareas = new List<Tarea>();

            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            
            string queryC = @"SELECT Tarea.id AS TareaId, Tarea.id_tablero, Tarea.nombre, Tarea.estado, Tarea.descripcion, Tarea.color, Tarea.id_usuario_asignado, Tarea.id_usuario_propietario, 
                            Tablero.nombre_tablero AS nombre_Tablero, 
                            UsuarioAsignado.nombre_de_usuario AS nombre_asignado, 
                            UsuarioPropietario.nombre_de_usuario AS nombre_propietario
                            FROM Tarea
                            INNER JOIN Tablero ON Tablero.id = Tarea.id_tablero
                            LEFT JOIN Usuario AS UsuarioAsignado ON UsuarioAsignado.id = Tarea.id_usuario_asignado
                            LEFT JOIN Usuario AS UsuarioPropietario ON UsuarioPropietario.id = Tarea.id_usuario_propietario
                            WHERE Tarea.id_usuario_propietario = @IDUSU;"; 
                            
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
                        var newTarea = new Tarea();
                        newTarea.Id = Convert.ToInt32(readerC["TareaId"]);
                        newTarea.Nombre = Convert.ToString(readerC["nombre"]);
                        newTarea.EstadoTarea = (EstadoTarea)Convert.ToInt32(readerC["estado"]);
                        newTarea.Descripcion = Convert.ToString(readerC["descripcion"]);
                        newTarea.Color = (Color)Convert.ToInt32(readerC["color"]);
                        newTarea.TableroPropio = new Tablero();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_tablero"))){
                            newTarea.TableroPropio.Id = Convert.ToInt32(readerC["id_tablero"]);
                            newTarea.TableroPropio.Nombre = Convert.ToString(readerC["nombre_Tablero"]);
                        }
                        newTarea.Asignado = new Usuario();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_asignado"))){
                            newTarea.Asignado.Id = Convert.ToInt32(readerC["id_usuario_asignado"]);
                            newTarea.Asignado.Nombre = Convert.ToString(readerC["nombre_asignado"]);
                        }
                        newTarea.Propietario = new Usuario();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_propietario"))){
                            newTarea.Propietario.Id = Convert.ToInt32(readerC["id_usuario_propietario"]);
                            newTarea.Propietario.Nombre = Convert.ToString(readerC["nombre_propietario"]);
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