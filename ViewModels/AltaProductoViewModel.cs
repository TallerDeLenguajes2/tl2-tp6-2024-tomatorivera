using System.ComponentModel.DataAnnotations;

namespace tl2_tp6_2024_tomatorivera.ViewModels;

public class AltaProductoViewModel
{
    private string _descripcion;
    private int _precio;

    public AltaProductoViewModel() 
    {
        _descripcion = string.Empty;
        _precio = 0;
    }

    [StringLength(250, ErrorMessage = "La longitud de {0} debe ser de máximo {1} caracteres")]
    public string Descripcion { get => _descripcion; set => _descripcion = value; }
    [Required(ErrorMessage = "El campo precio es requerido")]
    [Range(0, 9999999, ErrorMessage = "El valor del precio debe ser positivo")]
    public int Precio { get => _precio; set => _precio = value; }
}