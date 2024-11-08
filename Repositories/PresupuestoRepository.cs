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

            var sqlQuery = @"INSERT INTO Presupuestos (id_cliente, FechaCreacion) 
                             VALUES ($idCliente, $fechaCreacion)";
            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$idCliente", obj.Cliente.Id);
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

            var sqlQuery = @"SELECT P.idPresupuesto, P.FechaCreacion, C.id_cliente, C.nombre, C.email, C.telefono, PR.idProducto, PR.Descripcion, PR.Precio, PD.Cantidad
                             FROM Presupuestos P
                             INNER JOIN Clientes C USING (id_cliente)
                             LEFT JOIN PresupuestosDetalle PD USING (idPresupuesto)
                             LEFT JOIN Productos PR USING (idProducto)";

            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            using (var sqlReader = sqlCmd.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    // Genero el presupuesto si es que aún no lo generé (un mismo presupuesto
                    // puede aparecer varias veces por el join)
                    int idPresupuesto = sqlReader.GetInt32(0);
                    if (!presupuestos.TryGetValue(idPresupuesto, out var presupuesto))
                        presupuesto = generarPresupuesto(sqlReader);

                    // Genero los datos del cliente
                    presupuesto.Cliente = new Cliente(sqlReader.GetInt32(2), sqlReader.GetString(3), sqlReader.GetString(4), sqlReader.GetString(5));;

                    // Verifico si el presupuesto tiene detalle, de ser así, genero el
                    // objeto producto y detalle, y los agrego al presupuesto
                    if (!sqlReader.IsDBNull(6))
                    {
                        var producto = new Producto(sqlReader.GetInt32(6), sqlReader.GetString(7), sqlReader.GetInt32(8));
                        var detalle = new PresupuestoDetalle(sqlReader.GetInt32(9), producto);

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
                             SET id_cliente=$idCliente, FechaCreacion=$fechaCreacion 
                             WHERE idPresupuesto=$id";

            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$idCliente", obj.Cliente.Id);
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

            var sqlQuery = @"SELECT P.idPresupuesto, P.FechaCreacion, C.id_cliente, C.nombre, C.email, C.telefono, PR.idProducto, PR.Descripcion, PR.Precio, PD.Cantidad
                             FROM Presupuestos P
                             INNER JOIN Clientes C USING (id_cliente)
                             LEFT JOIN PresupuestosDetalle PD USING (idPresupuesto)
                             LEFT JOIN Productos PR USING (idProducto)
                             WHERE idPresupuesto=$id";

            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$id", id);
                using (var sqlReader = sqlCmd.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        // Genero el presupuesto si es que aún no está generado, al ser un JOIN
                        // sus datos se traen en cada fila
                        if (presupuestoBuscado.Id == -1)
                            presupuestoBuscado = generarPresupuesto(sqlReader);

                        // Genero los datos del cliente asociado al presupuesto
                        // si es que aún no lo he leído
                        if (presupuestoBuscado.Cliente.Id == -1)
                            presupuestoBuscado.Cliente = new Cliente(sqlReader.GetInt32(2), sqlReader.GetString(3), sqlReader.GetString(4), sqlReader.GetString(5));

                        // Verifico si el presupuesto tiene detalle, de ser así, genero el
                        // objeto producto y detalle, y los agrego al presupuesto
                        if (!sqlReader.IsDBNull(6))
                        {
                            var producto = new Producto(sqlReader.GetInt32(6), sqlReader.GetString(7), sqlReader.GetInt32(8));
                            var detalle = new PresupuestoDetalle(sqlReader.GetInt32(9), producto);

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
        return new Presupuesto(reader.GetInt32(0), reader.GetString(1));
    }
}