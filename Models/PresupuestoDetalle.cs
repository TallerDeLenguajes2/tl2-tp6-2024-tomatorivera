namespace Models;

public class PresupuestoDetalle
{
    private int cantidad;
    private Producto producto;

    public PresupuestoDetalle(int cantidad, Producto producto)
    {
        this.cantidad = cantidad;
        this.producto = producto;
    }

    public int Cantidad { get => cantidad; set => cantidad = value; }
    public Producto Producto { get => producto; set => producto = value; }
}