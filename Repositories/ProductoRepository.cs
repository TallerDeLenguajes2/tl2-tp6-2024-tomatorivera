using Microsoft.Data.Sqlite;
using Models;

namespace Persistence;

public class ProductoRepositoryImpl : IRepository<Producto>
{
    private readonly string connectionString = "Data Source=Db/Tienda.db;Cache=Shared";

    public void Insertar(Producto obj)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"INSERT INTO Productos (Descripcion, Precio) VALUES ($descripcion, $precio)";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$descripcion", obj.Descripcion);
                sqlCmd.Parameters.AddWithValue("$precio", obj.Precio);
                sqlCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public List<Producto> Listar()
    {
        var productos = new List<Producto>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"SELECT idProducto, Descripcion, Precio FROM Productos";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            using (var sqlReader = sqlCmd.ExecuteReader())
            {
                while (sqlReader.Read())
                    productos.Add(generarProducto(sqlReader));
            }

            connection.Close();
        }

        return productos;
    }

    public void Modificar(int id, Producto obj)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"UPDATE Productos 
                             SET Descripcion=$descripcion, Precio=$precio 
                             WHERE idProducto=$id";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$descripcion", obj.Descripcion);
                sqlCmd.Parameters.AddWithValue("$precio", obj.Precio);
                sqlCmd.Parameters.AddWithValue("$id", id);
                sqlCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void Eliminar(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"DELETE FROM Productos WHERE idProducto=$id";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$id", id);
                sqlCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public Producto Obtener(int id)
    {
        Producto producto;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto=$id";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$id", id);
                using (var sqlReader = sqlCmd.ExecuteReader())
                {
                    producto = sqlReader.Read() ? generarProducto(sqlReader) : new Producto();
                }
            }

            connection.Close();
        }

        return producto;
    }

    /// <summary>
    /// Genera un objeto <see cref="Producto"/> partir de los datos 
    /// de un SqliteDataReader generado por alguna consulta
    /// </summary>
    /// <param name="reader">Lector de alguna consulta</param>
    /// <returns>Nueva instancia de <see cref="Producto"/> con los datos
    /// traídos del reader o una instancia por defecto si es que ocurre algun error</returns>
    private Producto generarProducto(SqliteDataReader reader)
    {
        return new Producto(reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetInt32(2));
    }
}