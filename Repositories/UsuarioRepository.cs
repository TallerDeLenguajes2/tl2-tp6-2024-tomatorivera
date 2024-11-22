using Microsoft.Data.Sqlite;
using Models;

namespace Persistence;

public interface IUsuarioRepository
{
    Usuario GetUsuario(string username, string pass);
}

public class UsuarioRepositoryImpl : IUsuarioRepository
{
    private readonly string connectionString = "Data Source=Db/Tienda.db;Cache=Shared";

    public Usuario GetUsuario(string username, string pass)
    {
        Usuario? usuario = null;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            
            var sqlQuery = @"SELECT * FROM usuarios WHERE usuario=$usuario AND contraseña=$pass";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$usuario", username);
                sqlCmd.Parameters.AddWithValue("$pass", pass);
                using (var sqlReader = sqlCmd.ExecuteReader())
                {
                    usuario = sqlReader.Read() ? LeerUsuario(sqlReader)
                                               : new Usuario();
                }
            }

            connection.Close();
        }

        return usuario;
    }

    private Usuario LeerUsuario(SqliteDataReader sqlReader)
    {
        return new Usuario(Convert.ToInt32(sqlReader["id_usuario"]),
                           Convert.ToString(sqlReader["nombre"]) ?? string.Empty,
                           Convert.ToString(sqlReader["usuario"]) ?? string.Empty,
                           Convert.ToString(sqlReader["contraseña"]) ?? string.Empty,
                           Convert.ToString(sqlReader["rol"]) ?? string.Empty);
    }
}