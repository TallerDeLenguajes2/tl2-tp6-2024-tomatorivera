using Microsoft.Data.Sqlite;
using Models;
namespace Persistence;

public class ClienteRepositoryImpl : IRepository<Cliente>
{
    private readonly string connectionString = "Data Source=Db/Tienda.db;Cache=Shared";

    public void Insertar(Cliente obj)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"INSERT INTO Clientes (nombre, email, telefono) 
                             VALUES ($nombre, $email, $telefono)";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$nombre", obj.Nombre);
                sqlCmd.Parameters.AddWithValue("$email", obj.Email);
                sqlCmd.Parameters.AddWithValue("$telefono", obj.Telefono);
                sqlCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public List<Cliente> Listar()
    {
        var clientes = new List<Cliente>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"SELECT id_cliente, nombre, email, telefono FROM Clientes";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            using (var sqlReader = sqlCmd.ExecuteReader())
            {
                while (sqlReader.Read())
                    clientes.Add(new Cliente(sqlReader.GetInt32(0),
                                             sqlReader.GetString(1),
                                             sqlReader.GetString(2),
                                             sqlReader.GetString(3)));
            }

            connection.Close();
        }

        return clientes;
    }

    public Cliente Obtener(int id)
    {
         Cliente cliente;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"SELECT id_cliente, nombre, email, telefono FROM Clientes WHERE id_cliente=$id";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$id", id);
                using (var sqlReader = sqlCmd.ExecuteReader())
                {
                    cliente = sqlReader.Read() ? new Cliente(sqlReader.GetInt32(0), sqlReader.GetString(1), sqlReader.GetString(2), sqlReader.GetString(3)) 
                                               : new Cliente();
                }
            }

            connection.Close();
        }

        return cliente;
    }

    public void Eliminar(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"DELETE FROM Clientes WHERE id_cliente=$id";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$id", id);
                sqlCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void Modificar(int id, Cliente obj)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"UPDATE Clientes
                             SET nombre=$nombre, email=$email, telefono=$telefono 
                             WHERE id_cliente=$id";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$nombre", obj.Nombre);
                sqlCmd.Parameters.AddWithValue("$email", obj.Email);
                sqlCmd.Parameters.AddWithValue("$telefono", obj.Telefono);
                sqlCmd.Parameters.AddWithValue("$id", id);
                sqlCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
