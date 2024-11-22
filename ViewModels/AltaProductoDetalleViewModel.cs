using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

namespace tl2_tp6_2024_tomatorivera.ViewModels;

public class AltaProductoDetalleViewModel
{
    private IEnumerable<SelectListItem> _productos;
    private int _idProducto;
    private int _cantidad;
    private int _idPresupuesto;

    public AltaProductoDetalleViewModel()
    {
        _productos = new List<SelectListItem>();
        _idProducto = 0;
        _cantidad = 0;
        _idPresupuesto = 0;
    }
    
    public AltaProductoDetalleViewModel(int idPresupuesto, IEnumerable<SelectListItem> productos) : base()
    {
        _idPresupuesto = idPresupuesto;
        _productos = productos;
    }

    public IEnumerable<SelectListItem> Productos { 
        get => _productos;
        set {
            if (value is List<Producto> productos)
            {
                _productos = productos.Select(p => new SelectListItem {
                    Value = p.IdProducto.ToString(),
                    Text = p.Descripcion
                });
            }
            else
            {
                _productos = value;
            }
        }
    }
    public int IdProducto { get => _idProducto; set => _idProducto = value; }
    public int Cantidad { get => _cantidad; set => _cantidad = value; }
    public int IdPresupuesto { get => _idPresupuesto; set => _idPresupuesto = value; }
}