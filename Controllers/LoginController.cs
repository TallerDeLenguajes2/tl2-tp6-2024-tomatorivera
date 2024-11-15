using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tl2_tp6_2024_tomatorivera.Repositories;
using tl2_tp6_2024_tomatorivera.ViewModels;

namespace tl2_tp6_2024_tomatorivera.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _inMemoryUserRepository;

        public LoginController(IUserRepository inMemoryUserRepository)
        {
            _inMemoryUserRepository = inMemoryUserRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(
                new LoginViewModel
                {
                    IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true",
                    Username = HttpContext.Session.GetString("User") ?? string.Empty
                }
            );
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // Verifico que los datos no estén vacíos
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                model.ErrorMessage = "Por favor ingrese su nombre de usuario y contraseña.";
                return View("Index", model);
            }

            var usuario = _inMemoryUserRepository.GetUser(model.Username, model.Password);
            if (usuario != null)
            {
                // Creo la variable de sesión
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("User", usuario.Username);
                HttpContext.Session.SetString("AccessLevel", usuario.AccessLevel.ToString());

                // Redirige a la pag. principal
                return RedirectToAction("Index", "Home");
            }

            model.ErrorMessage = "Credenciales inválidas";
            model.IsAuthenticated = false;
            return RedirectToAction("Index", model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}