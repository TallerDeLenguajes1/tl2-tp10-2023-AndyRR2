using System.Data.SQLite;

using Proyecto.Models;

namespace Proyecto.Repositories{
    public class TareaRepository: ITareaRepository{
        private readonly string direccionBD;
        private readonly IUsuarioRepository repoUsuario;
        private readonly ITableroRepository repoTablero;
        public TareaRepository(string cadenaDeConexion, IUsuarioRepository usuRepo , ITableroRepository tabRepo)
        {
            direccionBD = cadenaDeConexion;
            repoUsuario = usuRepo;
            repoTablero = tabRepo;
        }
        public List<Tarea> GetAll(){
            List<Tarea> tareas = new List<Tarea>();
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = @"SELECT Tarea.id AS TareaId, Tarea.id_tablero, Tarea.nombre, Tarea.estado, Tarea.descripcion, Tarea.color, Tarea.id_usuario_asignado, Tarea.id_usuario_propietario, Tarea.nombre_tablero, Tarea.nombre_asignado, Tarea.nombre_propietario
    	                    FROM Tarea
                            LEFT JOIN Tablero ON Tablero.id = Tarea.id_tablero;";

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
                            newTarea.TableroPropio.Nombre = Convert.ToString(readerC["nombre_tablero"]);
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
            
            string queryC = @"SELECT Tarea.id AS TareaId, Tarea.id_tablero, Tarea.nombre, Tarea.estado, Tarea.descripcion, Tarea.color, Tarea.id_usuario_asignado, Tarea.id_usuario_propietario, Tarea.nombre_tablero, Tarea.nombre_asignado, Tarea.nombre_propietario
    	                    FROM Tarea WHERE Tarea.id_tablero = @IDTAB;";
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
                            newTarea.TableroPropio.Nombre = Convert.ToString(readerC["nombre_tablero"]);
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
                        newTarea.Id = Convert.ToInt32(readerC["id"]);
                        newTarea.Nombre = Convert.ToString(readerC["nombre"]);
                        newTarea.EstadoTarea = (EstadoTarea)Convert.ToInt32(readerC["estado"]);
                        newTarea.Descripcion = Convert.ToString(readerC["descripcion"]);
                        newTarea.Color = (Color)Convert.ToInt32(readerC["color"]);
                        newTarea.TableroPropio = new Tablero();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_tablero"))){
                            newTarea.TableroPropio.Id = Convert.ToInt32(readerC["id_tablero"]);
                            newTarea.TableroPropio.Nombre = Convert.ToString(readerC["nombre_tablero"]);
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

            string queryC = $"INSERT INTO Tarea (id_tablero,nombre,estado,descripcion,color,id_usuario_asignado,id_usuario_propietario,nombre_tablero,nombre_asignado,nombre_propietario) VALUES (@IDTAB,@NAME,@ESTADO,@DESCRIPCION,@COLOR,@IDUSUA,@IDUSUP,@NAMETAB,@NAMEASIG,@NAMEPROP)";
            SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",newTarea.Nombre);
            SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",newTarea.EstadoTarea);
            SQLiteParameter parameterDescripcion = new SQLiteParameter("@DESCRIPCION",newTarea.Descripcion);
            SQLiteParameter parameterColor = new SQLiteParameter("@COLOR",newTarea.Color);
            SQLiteParameter parameterIdUsuA = new SQLiteParameter("@IDUSUA",newTarea.Asignado.Id);
            SQLiteParameter parameterNombreA = new SQLiteParameter("@NAMEASIG",repoUsuario.GetById(newTarea.Asignado.Id).Nombre);
            SQLiteParameter parameterIdTab = new SQLiteParameter("@IDTAB",newTarea.TableroPropio.Id);
            SQLiteParameter parameterNombreTab = new SQLiteParameter("@NAMETAB",repoTablero.GetById(newTarea.TableroPropio.Id).Nombre);
            SQLiteParameter parameterIdUsuP = new SQLiteParameter("@IDUSUP",newTarea.Propietario.Id);
            SQLiteParameter parameterNombreP = new SQLiteParameter("@NAMEPROP",repoUsuario.GetById(newTarea.Propietario.Id).Nombre);
            
            using (connectionC)
            {
                connectionC.Open();
                var commandC = new SQLiteCommand(queryC, connectionC);
                commandC.Parameters.Add(parameterNombre);
                commandC.Parameters.Add(parameterEstado);
                commandC.Parameters.Add(parameterDescripcion);
                commandC.Parameters.Add(parameterColor);
                commandC.Parameters.Add(parameterIdTab);
                commandC.Parameters.Add(parameterNombreTab);
                commandC.Parameters.Add(parameterIdUsuA);
                commandC.Parameters.Add(parameterNombreA);
                commandC.Parameters.Add(parameterIdUsuP);
                commandC.Parameters.Add(parameterNombreP);
                
                commandC.ExecuteNonQuery();
                connectionC.Close();   
            }
            if (newTarea == null){
                throw new Exception("La Tarea no se creo correctamente.");
            }
        }
        public void Update(Tarea tareaAEditar){
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "UPDATE Tarea SET nombre = @NAME, descripcion = @DESCRIPCION, id_tablero = @IDTAB, estado = @ESTADO, color = @COLOR, id_usuario_propietario = @IDUSUP, nombre_propietario = @NAMEPROP, nombre_tablero = @NAMETAB WHERE id = @ID;";
            SQLiteParameter parameterId = new SQLiteParameter("@ID",tareaAEditar.Id);
            SQLiteParameter parameterIdTab = new SQLiteParameter("@IDTAB",tareaAEditar.TableroPropio.Id);
            SQLiteParameter parameterNombreTab = new SQLiteParameter("@NAMETAB",tareaAEditar.TableroPropio.Nombre);
            SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",tareaAEditar.Nombre);
            SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",tareaAEditar.EstadoTarea);
            SQLiteParameter parameterDescripcion = new SQLiteParameter("@DESCRIPCION",tareaAEditar.Descripcion);
            SQLiteParameter parameterColor = new SQLiteParameter("@COLOR",tareaAEditar.Color);
            SQLiteParameter parameterIdUsuP = new SQLiteParameter("@IDUSUP",tareaAEditar.Propietario.Id);
            SQLiteParameter parameterNombreProp = new SQLiteParameter("@NAMEPROP",tareaAEditar.Propietario.Nombre);
            using (connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterId);
                commandC.Parameters.Add(parameterIdTab);
                commandC.Parameters.Add(parameterNombreTab);
                commandC.Parameters.Add(parameterNombre);
                commandC.Parameters.Add(parameterEstado);
                commandC.Parameters.Add(parameterDescripcion);
                commandC.Parameters.Add(parameterColor);
                commandC.Parameters.Add(parameterIdUsuP);
                commandC.Parameters.Add(parameterNombreProp);

                int rowsAffected = commandC.ExecuteNonQuery();
                connectionC.Close();
                if (rowsAffected == 0){
                    throw new Exception("No se encontró ninguna tarea con el ID proporcionado.");
                }   
            }
        }
        /*public void Remove(int? idTarea){
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
                        }
                        newTarea.Nombre = Convert.ToString(readerC["nombre"]);
                        newTarea.EstadoTarea = (EstadoTarea)Convert.ToInt32(readerC["estado"]);
                        newTarea.Descripcion = Convert.ToString(readerC["descripcion"]);
                        newTarea.Color = (Color)Convert.ToInt32(readerC["color"]);
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_propietario"))){
                            newTarea.IdUsuarioPropietario = Convert.ToInt32(readerC["id_usuario_propietario"]);
                        }
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_asignado"))){
                            newTarea.IdUsuarioAsignado = Convert.ToInt32(readerC["id_usuario_asignado"]);
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
        }*/
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