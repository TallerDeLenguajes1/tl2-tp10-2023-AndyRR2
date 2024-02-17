using System.Data.SQLite;

using Proyecto.Models;

namespace Proyecto.Repositories{
    public class LoginRepository : ILoginRepository{
        private readonly string direccionBD;
        public LoginRepository(string cadenaDeConexion)
        {
            direccionBD = cadenaDeConexion;
        }
        public bool AutenticarUsuario(string? nombreUsuario, string? contrasenia)
        {
            bool validacion = false;
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "SELECT * FROM Usuario WHERE contrasenia = @PASS AND nombre_de_usuario = @USER";
            SQLiteParameter parameterUser = new SQLiteParameter("@USER", nombreUsuario);
            SQLiteParameter parameterPass = new SQLiteParameter("@PASS", contrasenia);

            using (connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterUser);
                commandC.Parameters.Add(parameterPass);

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
            return validacion;
        }
        public Usuario ObtenerUsuario(string? nombreUsuario, string? contrasenia)
        {
            Usuario usuarioPorLoguear = new Usuario();
            
            SQLiteConnection connectionC = new SQLiteConnection(direccionBD);

            string queryC = "SELECT * FROM Usuario WHERE contrasenia = @PASS AND nombre_de_usuario = @USER";
            SQLiteParameter parameterUser = new SQLiteParameter("@USER", nombreUsuario);
            SQLiteParameter parameterPass = new SQLiteParameter("@PASS", contrasenia);

            using (connectionC)
            {
                connectionC.Open();
                SQLiteCommand commandC = new SQLiteCommand(queryC,connectionC);
                commandC.Parameters.Add(parameterUser);
                commandC.Parameters.Add(parameterPass);

                SQLiteDataReader readerC = commandC.ExecuteReader();
                using (readerC)
                {
                    while (readerC.Read())
                    {
                        usuarioPorLoguear.Contrasenia = Convert.ToString(readerC["contrasenia"]);
                        usuarioPorLoguear.Nombre = Convert.ToString(readerC["nombre_de_usuario"]);
                        usuarioPorLoguear.NivelDeAcceso = (NivelDeAcceso)Convert.ToInt16(readerC["nivel_de_acceso"]); //convierte de string a enum
                        usuarioPorLoguear.Id = Convert.ToInt16(readerC["id"]);
                    }
                }
                connectionC.Close();
            }
            if (usuarioPorLoguear == null)
            {
                throw new Exception("No se encontro el usuario en la base de datos.");
            }
            return(usuarioPorLoguear);
        }
    }
}