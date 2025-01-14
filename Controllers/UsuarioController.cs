using Microsoft.AspNetCore.Mvc;
using Models;
using Persistence;
using tl2_tp6_2024_tomatorivera.ViewModels;

namespace tl2_tp6_2024_tomatorivera.Controllers;

public class UsuarioController : Controller {

    private readonly ILogger<ClienteController> _logger;
    private readonly IUsuarioRepository _repositorioUsuarios;
    
    public UsuarioController(ILogger<ClienteController> logger, IUsuarioRepository repositorioUsuarios)
    {
        _logger = logger;
        _repositorioUsuarios = repositorioUsuarios;
    }
    
    [HttpGet]
    public IActionResult Login() {
        return View(
            new LoginViewModel {
                IsAuth = HttpContext.Session.GetString("IsAuth") == "true",
                Username = HttpContext.Session.GetString("User") ?? string.Empty
            }
        );
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model) {
        if (!ModelState.IsValid) 
        {
            model.IsAuth = false;
            return View(model);
        }

        if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password)) 
        {
            model.ErrorMessage = "Los campos no pueden estar vacíos";
            return View(model);
        }

        Usuario user = _repositorioUsuarios.GetUsuario(model.Username, model.Password);
        if (user.Id == 0)
        {
            model.ErrorMessage = "Credenciales inválidas";
            return View(model);
        }

        // Creo la variable de sesión
        HttpContext.Session.SetString("IsAuth", "true");
        HttpContext.Session.SetString("User", user.Username);
        HttpContext.Session.SetInt32("AccessLevel", user.Rol);

        // Redirige a la pag. principal
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}