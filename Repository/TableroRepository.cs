namespace EspacioTableroRepository;

using System.Data.SQLite;

using Tp11.Models;
using EspacioTareaRepository;

public class TableroRepository : ITableroRepository{
    private readonly string direccionBD = "Data Source = DataBase/kamban.db;Cache=Shared"; 

    public void Create(Tablero newTablero){
        string queryC = $"INSERT INTO Tablero (id,id_usuario_propietario,nombre_tablero,descripcion,estado) VALUES(@ID,@IDUSU,@NAME,@DESCRIPCION,@ESTADO)";
        SQLiteParameter parameterId = new SQLiteParameter("@ID",newTablero.Id);
        SQLiteParameter parameterIdUsu = new SQLiteParameter("@IDUSU",newTablero.IdUsuarioPropietario);
        SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",newTablero.Nombre);
        SQLiteParameter parameterDescripcion = new SQLiteParameter("@DESCRIPCION",newTablero.Descripcion);
        SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",newTablero.Estado);

        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
        using (connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterId);
            commandC.Parameters.Add(parameterIdUsu);
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
        
        string queryC = "UPDATE Tablero SET id_usuario_propietario = @IDUSU, nombre_tablero = @NAME, descripcion = @DESCRIPCION, estado = @ESTADO WHERE id = @ID";
        SQLiteParameter parameterId = new SQLiteParameter("@ID",newTablero.Id);
        SQLiteParameter parameterIdUsu = new SQLiteParameter("@IDUSU",newTablero.IdUsuarioPropietario);
        SQLiteParameter parameterNombre = new SQLiteParameter("@NAME",newTablero.Nombre);
        SQLiteParameter parameterDescripcion = new SQLiteParameter("@DESCRIPCION",newTablero.Descripcion);
        SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",newTablero.Estado);

        using (connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterId);
            commandC.Parameters.Add(parameterIdUsu);
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
                    tableroSelec.IdUsuarioPropietario= Convert.ToInt32(readerC["id_usuario_propietario"]);
                    tableroSelec.Nombre = Convert.ToString(readerC["nombre_tablero"]);
                    tableroSelec.Descripcion = Convert.ToString(readerC["descripcion"]);
                    tableroSelec.Estado = (EstadoTablero)Convert.ToInt32(readerC["estado"]);
                }
            }
            connectionC.Close();
        }
        if (tableroSelec==null){
            throw new Exception("El Tablero no esta creado.");
        }
        return(tableroSelec);
    }
    public List<Tablero> GetAll(){
        List<Tablero> tableros = new List<Tablero>();
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

        string queryC = "SELECT * FROM Tablero;";
        
        using(connectionC){
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            
            SQLiteDataReader readerC = commandC.ExecuteReader();
            using (readerC)
            {
                while (readerC.Read())
                {
                    Tablero tableroPorAgregar = new Tablero();
                    tableroPorAgregar.Id = Convert.ToInt32(readerC["id"]);
                    tableroPorAgregar.IdUsuarioPropietario = Convert.ToInt32(readerC["id_usuario_propietario"]);
                    tableroPorAgregar.Nombre = Convert.ToString(readerC["nombre_tablero"]);
                    tableroPorAgregar.Descripcion = Convert.ToString(readerC["descripcion"]);
                    tableroPorAgregar.Estado = (EstadoTablero)Convert.ToInt32(readerC["estado"]);
                    tableros.Add(tableroPorAgregar);
                }   
            }
            connectionC.Close();
        }
        if (tableros==null){
            throw new Exception("Lista de tableros no encontrada.");
        }
        return(tableros);
    }
    public List<Tablero> GetTablerosDeUsuario(int? idUsuario){
        List<Tablero> tableros = new List<Tablero>();
        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
        
        string queryC = "SELECT * FROM Tablero WHERE id_usuario_propietario = @IDUSU OR id IN (SELECT id_tablero FROM Tarea WHERE id_usuario_asignado = @IDUSU)";
        SQLiteParameter parameterIdUsu = new SQLiteParameter("@IDUSU",idUsuario);

        using(connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterIdUsu);

            SQLiteDataReader readerC = commandC.ExecuteReader();
            using (readerC)
            {
                while (readerC.Read())
                {
                    Tablero newTablero = new Tablero();
                    newTablero.Id = Convert.ToInt32(readerC["id"]);
                    newTablero.IdUsuarioPropietario = Convert.ToInt32(readerC["id_usuario_propietario"]);
                    newTablero.Nombre = Convert.ToString(readerC["nombre_tablero"]);
                    newTablero.Descripcion = Convert.ToString(readerC["descripcion"]);
                    newTablero.Estado = (EstadoTablero)Convert.ToInt32(readerC["estado"]);
                    tableros.Add(newTablero);
                }
            }
            connectionC.Close();           
        }
        if (tableros == null){
            throw new Exception("El usuario proporcionado no tiene tableros.");
        }
        return(tableros);
    }
    public void Remove(int? idTablero){
        TareaRepository repoT = new TareaRepository();
        repoT.InhabilitarDeTablero(idTablero);

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

    public void Inhabilitar(int? IdUsuario){
        TareaRepository repoT = new TareaRepository();
        repoT.InhabilitarDeUsuario(IdUsuario);

        SQLiteConnection connectionC = new SQLiteConnection(direccionBD);
        
        string queryC = "UPDATE Tablero SET estado = @ESTADO WHERE id_usuario_propietario = @ID";
        SQLiteParameter parameterId = new SQLiteParameter("@ID",IdUsuario);
        SQLiteParameter parameterEstado = new SQLiteParameter("@ESTADO",2);

        using (connectionC)
        {
            connectionC.Open();
            SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
            commandC.Parameters.Add(parameterId);
            commandC.Parameters.Add(parameterEstado);

            int rowAffected =  commandC.ExecuteNonQuery();
            connectionC.Close();
            if (rowAffected == 0){
                throw new Exception("No se encontró ningún tablero de ese usuario.");
            }
        }
    }
}