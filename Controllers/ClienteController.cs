using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Persistence;

namespace tl2_tp6_2024_tomatorivera.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IRepository<Cliente> _repositorioClientes;
        
        public ClienteController(ILogger<ClienteController> logger)
        {
            _logger = logger;
            _repositorioClientes = new ClienteRepositoryImpl();
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
        public IActionResult Alta([FromForm] Cliente cliente)
        {
            _repositorioClientes.Insertar(cliente);
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