using System.ComponentModel.DataAnnotations;

namespace tl2_tp6_2024_tomatorivera.ViewModels;

public class AltaProductoViewModel
{
    [Required(ErrorMessage = "El campo descripción es requerido")]
    private string _descripcion;
    [Required(ErrorMessage = "El campo precio es requerido")]
    private int _precio;

    public AltaProductoViewModel() 
    {
        _descripcion = string.Empty;
        _precio = 0;
    }

    public string Descripcion { get => _descripcion; set => _descripcion = value; }
    public int Precio { get => _precio; set => _precio = value; }
}