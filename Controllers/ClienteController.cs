using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Persistence;
using tl2_tp6_2024_tomatorivera.ViewModels;

namespace tl2_tp6_2024_tomatorivera.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IRepository<Cliente> _repositorioClientes;
        
        public ClienteController(ILogger<ClienteController> logger, IRepository<Cliente> repositorioClientes)
        {
            _logger = logger;
            _repositorioClientes = repositorioClientes;
        }

        [HttpGet]
        public IActionResult Listar() {
            return View(_repositorioClientes.Listar());
        }

        [HttpGet]
        public IActionResult Alta() {
            return View();
        }

        [HttpPost]
        public IActionResult Alta([FromForm] AltaClienteViewModel clienteViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _repositorioClientes.Insertar(new Cliente(clienteViewModel.Nombre, clienteViewModel.Email, clienteViewModel.Telefono));
            return RedirectToAction("Listar");
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            _repositorioClientes.Eliminar(id);
            return RedirectToAction("Listar");
        }

        [HttpGet]
        public IActionResult Modificar(int id)
        {
            return View(_repositorioClientes.Obtener(id));
        }

        [HttpPost]
        public IActionResult Modificar([FromForm] Cliente cliente)
        {
            _repositorioClientes.Modificar(cliente.Id, cliente);
            return RedirectToAction("Listar");
        }
    }
}