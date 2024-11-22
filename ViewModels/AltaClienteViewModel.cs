using System.ComponentModel.DataAnnotations;

namespace tl2_tp6_2024_tomatorivera.ViewModels;

public class AltaClienteViewModel
{
    private string _nombre;
    private string _email;
    private string _telefono;

    public AltaClienteViewModel()
    {
        _nombre = string.Empty;
        _email = string.Empty;
        _telefono = string.Empty;
    }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Nombre { get => _nombre; set => _nombre = value; }
    [Required(ErrorMessage = "Este campo es obligatorio")]
    [EmailAddress(ErrorMessage = "Debe ingresar un mail válido")]
    public string Email { get => _email; set => _email = value; }
    [Required(ErrorMessage = "Este campo es obligatorio")]
    [Phone(ErrorMessage = "Debe ingresar un teléfono válido")]
    public string Telefono { get => _telefono; set => _telefono = value; }
}