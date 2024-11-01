namespace Models;

public class Presupuesto
{
    private static float IVA = 0.21f;

    private int id;
    private string nombreDestinatario;
    private string fechaCreacion;
    private List<PresupuestoDetalle> detalle;

    public Presupuesto()
    {
        id = -1;
        nombreDestinatario = string.Empty;
        fechaCreacion = string.Empty;
        detalle = new List<PresupuestoDetalle>();
    }

    public Presupuesto(string nombreDestinatario, string fechaCreacion)
    {
        this.nombreDestinatario = nombreDestinatario;
        this.fechaCreacion = fechaCreacion;
        id = -1;
        detalle = new List<PresupuestoDetalle>();
    }

    public Presupuesto(int idPresupuesto, string nombreDestinatario, string fechaCreacion)
    {
        this.id = idPresupuesto;
        this.nombreDestinatario = nombreDestinatario;
        this.fechaCreacion = fechaCreacion;
        this.detalle = new List<PresupuestoDetalle>();
    }

    public int Id { get => id; set => id = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public string FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }

    public float MontoPresupuesto()
    {
        return detalle.Select(d => d.Producto.Precio * d.Cantidad).Sum();
    }

    public float MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * (1 + IVA);
    }

    public int CantidadProductos()
    {
        return detalle.Select(d => d.Cantidad).Sum();
    }

    public override bool Equals(object? obj)
    {
        // Verifica si el objeto comparado es null o de un tipo diferente
        if (obj == null || GetType() != obj.GetType())
            return false;

        // Castea el objeto al tipo de tu clase
        Presupuesto other = (Presupuesto)obj;

        // Compara los campos relevantes
        return id == other.id;
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}