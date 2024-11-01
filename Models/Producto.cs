namespace Models;

public class Producto
{
    private int idProducto;
    private string descripcion;
    private int precio;

    public Producto()
    {
        idProducto = -1;
        descripcion = string.Empty;
        precio = 0;
    }

    public Producto(string descripcion, int precio)
    {
        idProducto = 0;
        this.descripcion = descripcion;
        this.precio = precio;
    }

    public Producto(int idProducto, string descripcion, int precio)
    {
        this.idProducto = idProducto;
        this.descripcion = descripcion;
        this.precio = precio;
    }

    public int IdProducto { get => idProducto; set => idProducto = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public int Precio { get => precio; set => precio = value; }
}