using System.ComponentModel.DataAnnotations;

namespace tl2_tp6_2024_tomatorivera.ViewModels;

public class LoginViewModel {

    [Required(ErrorMessage = "Este campo no puede estar vacío")]
    [MaxLength(15, ErrorMessage = "El username debe tener máximo {1} caracteres")]
    [MinLength(5, ErrorMessage = "El username debe tener como mínimo {1} caracteres")] 
    public string Username { get; set; } = string.Empty;
    [Required(ErrorMessage = "Este campo no puede estar vacío")]
    [MaxLength(15, ErrorMessage = "El username debe tener máximo {1} caracteres")]
    [MinLength(5, ErrorMessage = "El username debe tener como mínimo {1} caracteres")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public bool IsAuth { get; set; }

}