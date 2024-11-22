using Microsoft.Data.Sqlite;
using Models;

namespace Persistence;

public interface IUsuarioRepository
{
    Usuario GetUsuario(string username, string pass);
    void Insertar(Usuario usuario);
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

    public void Insertar(Usuario usuario)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            
            var sqlQuery = @"INSERT INTO usuarios (nombre, usuario, contraseña, rol)
                             VALUES ($nombre, $usuario, $contraseña, $rol)";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$nombre", usuario.Nombre);
                sqlCmd.Parameters.AddWithValue("$usuario", usuario.Username);
                sqlCmd.Parameters.AddWithValue("$contraseña", usuario.Contrasenia);
                sqlCmd.Parameters.AddWithValue("$rol", usuario.Rol);
                sqlCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
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