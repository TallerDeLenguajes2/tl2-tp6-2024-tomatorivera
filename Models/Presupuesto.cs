namespace Models;

public class Presupuesto
{
    public static float IVA = 0.21f;

    private int id;
    private Cliente cliente;
    private string fechaCreacion;
    private List<PresupuestoDetalle> detalle;

    public Presupuesto()
    {
        id = -1;
        cliente = new Cliente();
        fechaCreacion = string.Empty;
        detalle = new List<PresupuestoDetalle>();
    }

    public Presupuesto(int id, string fechaCreacion) : this()
    {
        this.id = id;
        this.fechaCreacion = fechaCreacion;
    }

    public Presupuesto(Cliente cliente, string fechaCreacion) : this()
    {
        this.cliente = cliente;
        this.fechaCreacion = fechaCreacion;
    }

    public int Id { get => id; set => id = value; }
    public string FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }
    public Cliente Cliente { get => cliente; set => cliente = value; }

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