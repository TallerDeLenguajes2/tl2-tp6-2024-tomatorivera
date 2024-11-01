using Microsoft.Data.Sqlite;
using Models;

namespace Persistence;

public interface IPresupuestoRepository : IRepository<Presupuesto>
{
    void InsertarDetalle(int idPresupuesto, int idProducto, int cantidad);
}

public class PresupuestoRepositoryImpl : IPresupuestoRepository
{
    private readonly string connectionString = "Data Source=Db/Tienda.db;Cache=Shared";

    public void Insertar(Presupuesto obj)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) 
                             VALUES ($nombreDestinatario, $fechaCreacion)";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$nombreDestinatario", obj.NombreDestinatario);
                sqlCmd.Parameters.AddWithValue("$fechaCreacion", obj.FechaCreacion);
                sqlCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public List<Presupuesto> Listar()
    {
        // el JOIN de sqlQuery me devuelve el mismo producto con distintos detalles en distintas lineas
        // por eso utilizo el diccionario para reconocer cuando ya se ha traído un producto
        var presupuestos = new Dictionary<int, Presupuesto>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"SELECT p.idPresupuesto, p.NombreDestinatario, p.FechaCreacion, pr.idProducto, pr.Descripcion, pr.Precio, pd.Cantidad 
                             FROM Presupuestos p
                             LEFT JOIN PresupuestosDetalle pd USING (idPresupuesto)
                             LEFT JOIN Productos pr USING (idProducto)";

            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            using (var sqlReader = sqlCmd.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    int idPresupuesto = sqlReader.GetInt32(0);

                    if (!presupuestos.TryGetValue(idPresupuesto, out var presupuesto))
                        presupuesto = generarPresupuesto(sqlReader);

                    if (!sqlReader.IsDBNull(3))
                    {
                        var producto = new Producto(sqlReader.GetInt32(3), sqlReader.GetString(4), sqlReader.GetInt32(5));
                        var detalle = new PresupuestoDetalle(sqlReader.GetInt32(6), producto);

                        presupuesto.Detalle.Add(detalle);
                    }

                    presupuestos.TryAdd(idPresupuesto, presupuesto);
                }
            }

            connection.Close();
        }

        return new List<Presupuesto>(presupuestos.Values);
    }

    public void Eliminar(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"DELETE FROM Presupuestos WHERE idPresupuesto=$id";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$id", id);
                sqlCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void Modificar(int id, Presupuesto obj)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"UPDATE Presupuestos 
                             SET NombreDestinatario=$nombreDestinatario, FechaCreacion=$fechaCreacion 
                             WHERE idPresupuesto=$id";

            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$nombreDestinatario", obj.NombreDestinatario);
                sqlCmd.Parameters.AddWithValue("$fechaCreacion", obj.FechaCreacion);
                sqlCmd.Parameters.AddWithValue("$id", obj.Id);
                sqlCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public Presupuesto Obtener(int id)
    {
        Presupuesto presupuestoBuscado = new Presupuesto();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"SELECT p.idPresupuesto, p.NombreDestinatario, p.FechaCreacion, pr.idProducto, pr.Descripcion, pr.Precio, pd.Cantidad
                             FROM Presupuestos p 
                             LEFT JOIN PresupuestosDetalle pd USING (idPresupuesto)
                             LEFT JOIN Productos pr USING (idProducto)
                             WHERE idPresupuesto=$id";

            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$id", id);
                using (var sqlReader = sqlCmd.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (presupuestoBuscado.Id == -1)
                            presupuestoBuscado = generarPresupuesto(sqlReader);

                        if (!sqlReader.IsDBNull(3))
                        {
                            var producto = new Producto(sqlReader.GetInt32(3), sqlReader.GetString(4), sqlReader.GetInt32(5));
                            var detalle = new PresupuestoDetalle(sqlReader.GetInt32(6), producto);
                            presupuestoBuscado.Detalle.Add(detalle);
                        }
                    }
                }
            }

            connection.Close();
        }

        return presupuestoBuscado;
    }

    public void InsertarDetalle(int idPresupuesto, int idProducto, int cantidad)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad)
                             VALUES ($idPresupuesto, $idProducto, $cantidad)";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$idPresupuesto", idPresupuesto);
                sqlCmd.Parameters.AddWithValue("$idProducto", idProducto);
                sqlCmd.Parameters.AddWithValue("$cantidad", cantidad);
                sqlCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    /// <summary>
    /// Genera un objeto <see cref="Presupuesto"/> partir de los datos 
    /// de un SqliteDataReader generado por alguna consulta
    /// </summary>
    /// <param name="reader">Lector de alguna consulta</param>
    /// <returns>Nueva instancia de <see cref="Presupuesto"/> con los datos
    /// traídos del reader o una instancia por defecto si es que ocurre algun error</returns>
    private Presupuesto generarPresupuesto(SqliteDataReader reader)
    {
        try
        {
            return new Presupuesto(reader.GetInt32(0),
                                   reader.GetString(1),
                                   reader.GetString(2));
        }
        catch (Exception)
        {
            return new Presupuesto();
        }
    }
}