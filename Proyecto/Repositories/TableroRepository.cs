using System.Data.SQLite;

using Proyecto.Models;

namespace Proyecto.Repositories{
    public class TableroRepository: ITableroRepository{
        private readonly string direccionBD;
        private readonly IUsuarioRepository repoUsuario;
        public TableroRepository(string cadenaDeConexion, IUsuarioRepository usuRepo)
        {
            direccionBD = cadenaDeConexion;
            repoUsuario = usuRepo;
        }
        public List<Tablero> GetAll(){
            List<Tablero> tableros = new List<Tablero>();
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = @"SELECT Tablero.id AS TableroId, id_usuario_propietario, nombre_tablero, nombre_propietario, descripcion, estado 
    	                    FROM Tablero
                            LEFT JOIN Usuario ON Tablero.id_usuario_propietario = Usuario.id;";
            using(connectionC){
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                
                SQLiteDataReader readerC = commandC.ExecuteReader();
                using (readerC)
                {
                    while (readerC.Read())
                    {
                        Tablero tableroPorAgregar = new Tablero();
                        tableroPorAgregar.Id = Convert.ToInt32(readerC["TableroId"]);
                        tableroPorAgregar.Propietario = new Usuario();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_propietario"))){
                            tableroPorAgregar.Propietario.Id = Convert.ToInt32(readerC["id_usuario_propietario"]);
                            tableroPorAgregar.Propietario.Nombre = Convert.ToString(readerC["nombre_propietario"]);
                        }
                        tableroPorAgregar.Nombre = Convert.ToString(readerC["nombre_tablero"]);
                        tableroPorAgregar.Descripcion = Convert.ToString(readerC["descripcion"]);
                        tableroPorAgregar.EstadoTablero = (EstadoTablero)Convert.ToInt32(readerC["estado"]);
                        tableros.Add(tableroPorAgregar);
                    }   
                }
                connectionC.Close();
            }
            if (tableros==null){
                throw new Exception("No se encontraron tableros en la base de datos.");
            }
            return(tableros);
        }
        public List<Tablero> GetAllByOwnerUser(int? idUsuario){
            List<Tablero> tableros = new List<Tablero>();
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = @"SELECT Tablero.id AS TableroId, id_usuario_propietario, nombre_tablero, nombre_propietario, descripcion, estado 
    	                    FROM Tablero WHERE Tablero.id_usuario_propietario = @ID;";
            SQLiteParameter parameterId = new SQLiteParameter("@ID",idUsuario);
            
            using(connectionC){
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterId);
                
                SQLiteDataReader readerC = commandC.ExecuteReader();
                using (readerC)
                {
                    while (readerC.Read())
                    {
                        Tablero tableroPorAgregar = new Tablero();
                        tableroPorAgregar.Id = Convert.ToInt32(readerC["TableroId"]);
                        tableroPorAgregar.Propietario = new Usuario();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_propietario"))){
                            tableroPorAgregar.Propietario.Id = Convert.ToInt32(readerC["id_usuario_propietario"]);
                            tableroPorAgregar.Propietario.Nombre = Convert.ToString(readerC["nombre_propietario"]);
                        }
                        tableroPorAgregar.Nombre = Convert.ToString(readerC["nombre_tablero"]);
                        tableroPorAgregar.Descripcion = Convert.ToString(readerC["descripcion"]);
                        tableroPorAgregar.EstadoTablero = (EstadoTablero)Convert.ToInt32(readerC["estado"]);
                        tableros.Add(tableroPorAgregar);
                    }   
                }
                connectionC.Close();
            }
            if (tableros==null){
                throw new Exception("No se encontraron tableros en la base de datos.");
            }
            return(tableros);
        }
        public List<Tablero> GetAllByAsignedTask(int? idUsuario){
            List<Tablero> tableros = new List<Tablero>();
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = @"SELECT DISTINCT Tablero.id AS TableroId, Tablero.id_usuario_propietario, Tablero.nombre_tablero, Tablero.nombre_propietario, Tablero.descripcion, Tablero.estado 
                            FROM Tablero
                            INNER JOIN Tarea ON Tablero.id = Tarea.id_tablero  
                            WHERE Tarea.id_usuario_asignado = @ID OR Tarea.id_usuario_propietario = @ID
                            GROUP BY Tablero.id;";
            SQLiteParameter parameterId = new SQLiteParameter("@ID",idUsuario);

            using(connectionC){
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterId);

                SQLiteDataReader readerC = commandC.ExecuteReader();
                using (readerC)
                {
                    while (readerC.Read())
                    {
                        Tablero tableroPorAgregar = new Tablero();
                        tableroPorAgregar.Id = Convert.ToInt32(readerC["TableroId"]);
                        tableroPorAgregar.Propietario = new Usuario();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_propietario"))){
                            tableroPorAgregar.Propietario.Id = Convert.ToInt32(readerC["id_usuario_propietario"]);
                            tableroPorAgregar.Propietario.Nombre = Convert.ToString(readerC["nombre_propietario"]);
                        }
                        tableroPorAgregar.Nombre = Convert.ToString(readerC["nombre_tablero"]);
                        tableroPorAgregar.Descripcion = Convert.ToString(readerC["descripcion"]);
                        tableroPorAgregar.EstadoTablero = (EstadoTablero)Convert.ToInt32(readerC["estado"]);
                        tableros.Add(tableroPorAgregar);
                    }   
                }
                connectionC.Close();
            }
            if (tableros==null){
                throw new Exception("No se encontraron tableros en la base de datos.");
            }
            return(tableros);
        }
        public Tablero GetById(int? Id){
            Tablero tableroSelec = new Tablero();
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            
            string queryC = "SELECT * FROM Tablero WHERE id = @ID";
            SQLiteParameter parameterId = new SQLiteParameter("@ID",Id);
            
            using(connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterId);

                SQLiteDataReader readerC = commandC.ExecuteReader();
                using (readerC)
                {
                    while (readerC.Read())
                    {
                        tableroSelec.Id = Convert.ToInt32(readerC["id"]);
                        tableroSelec.Propietario = new Usuario();
                        if (!readerC.IsDBNull(readerC.GetOrdinal("id_usuario_propietario"))){
                            tableroSelec.Propietario.Id = Convert.ToInt32(readerC["id_usuario_propietario"]);
                            tableroSelec.Propietario.Nombre = Convert.ToString(readerC["nombre_propietario"]);
                        }
                        tableroSelec.Nombre = Convert.ToString(readerC["nombre_tablero"]);
                        tableroSelec.Descripcion = Convert.ToString(readerC["descripcion"]);
                        tableroSelec.EstadoTablero = (EstadoTablero)Convert.ToInt32(readerC["estado"]);
                    }
                }
                connectionC.Close();
            }
            if (tableroSelec==null){
                throw new Exception("No se encontro el tablero con el id proporcionado en la base de datos.");
            }
            return(tableroSelec);
        }

        public void Create(Tablero newTablero){
            if (BoardExists(newTablero.Nombre))
            {
                throw new Exception("El Tablero ya existe.");
            }
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = $"INSERT INTO Tablero (id_usuario_propietario,nombre_propietario,nombre_tablero,descripcion,estado) VALUES(@IDUSU,@NAMEUSU,@NAME,@DESCRIPCION,@ESTADO)";
            SQLiteParameter parameterIdUsu = new SQLiteParameter("@IDUSU",newTablero.Propietario.Id);
            SQLiteParameter parameterNameUsu = new SQLiteParameter("@NAMEUSU",repoUsuario.GetById(newTablero.Propietario.Id).Nombre);
            SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",newTablero.Nombre);
            SQLiteParameter parameterDescripcion = new SQLiteParameter("@DESCRIPCION",newTablero.Descripcion);
            SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",newTablero.EstadoTablero);

            using (connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterIdUsu);
                commandC.Parameters.Add(parameterNameUsu);
                commandC.Parameters.Add(parameterNombre);
                commandC.Parameters.Add(parameterDescripcion);
                commandC.Parameters.Add(parameterEstado);
                commandC.ExecuteNonQuery();
                connectionC.Close();
            }
            if (newTablero==null){
                throw new Exception("El Tablero no se creo correctamente.");
            }
        }
        public void Update(Tablero newTablero){
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            
            string queryC = "UPDATE Tablero SET id_usuario_propietario = @IDUSU, nombre_tablero = @NAME, descripcion = @DESCRIPCION, estado = @ESTADO, nombre_propietario = @NAMEUSU WHERE id = @ID;";
            SQLiteParameter parameterId = new SQLiteParameter("@ID",newTablero.Id);
            SQLiteParameter parameterIdUsu = new SQLiteParameter("@IDUSU",newTablero.Propietario.Id);
            SQLiteParameter parameterNameUsu = new SQLiteParameter("@NAMEUSU",repoUsuario.GetById(newTablero.Propietario.Id).Nombre);
            SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",newTablero.Nombre);
            SQLiteParameter parameterDescripcion = new SQLiteParameter("@DESCRIPCION",newTablero.Descripcion);
            SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",newTablero.EstadoTablero);

            using (connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterId);
                commandC.Parameters.Add(parameterIdUsu);
                commandC.Parameters.Add(parameterNameUsu);
                commandC.Parameters.Add(parameterNombre);
                commandC.Parameters.Add(parameterDescripcion);
                commandC.Parameters.Add(parameterEstado);

                int rowAffected =  commandC.ExecuteNonQuery();
                connectionC.Close();
                if (rowAffected == 0){
                    throw new Exception("No se encontró ningún tablero con el ID proporcionado.");
                }
            }
        }
        public void Remove(int? idTablero){

            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "DELETE FROM Tablero WHERE id = @ID";
            SQLiteParameter parameterId = new SQLiteParameter("@ID",idTablero);

            using(connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterId);

                int rowAffected =  commandC.ExecuteNonQuery();
                connectionC.Close();
                if (rowAffected == 0){
                    throw new Exception("No se encontró ningún tablero con el ID proporcionado.");
                }
            }
        }
        public void Disable(int? idTablero){

            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
            
            string queryC = @"UPDATE Tablero SET estado = @ESTADO, id_usuario_propietario = NULL, nombre_propietario = NULL WHERE id = @ID;

                            UPDATE Tarea SET estado = @ESTADOT WHERE id_tablero = @ID;";

            SQLiteParameter parameterId = new SQLiteParameter("@ID",idTablero);
            SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",2);
            SQLiteParameter parameterEstadoT = new SQLiteParameter("@ESTADOT",6);

            using (connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterId);
                commandC.Parameters.Add(parameterEstado);

                int rowAffected =  commandC.ExecuteNonQuery();
                connectionC.Close();
                if (rowAffected == 0){
                    throw new Exception("No se encontró ningún tablero con el ID proporcionado.");
                }
            }
        }
        
        public bool ChechAsignedTask(int? idTablero, int? idUsuario){
            bool validacion = false;
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "SELECT * FROM Tarea WHERE id_usuario_asignado = @IDASIGN OR id_tablero = @IDTAB";
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
        
        public bool BoardExists(string? nombreTablero){
            bool validacion=false;
            string? Nombre=null;
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "SELECT * FROM Tablero WHERE nombre_tablero = @NAME";
            SQLiteParameter parameterName = new SQLiteParameter("@NAME",nombreTablero);

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
                        Nombre = Convert.ToString(readerC["nombre_tablero"]);
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