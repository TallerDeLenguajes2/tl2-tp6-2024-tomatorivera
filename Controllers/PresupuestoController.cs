using Microsoft.AspNetCore.Mvc;
using Models;
using Persistence;

namespace tl2_tp6_2024_tomatorivera.Controllers;

public class PresupuestoController : Controller
{
    private readonly ILogger<PresupuestoController> _logger;
    private readonly IRepository<Presupuesto> repositorioPresupuestos;

    public PresupuestoController(ILogger<PresupuestoController> logger)
    {
        _logger = logger;
        repositorioPresupuestos = new PresupuestoRepositoryImpl();
    }

    [HttpGet]
    public IActionResult Listar() {
        return View(repositorioPresupuestos.Listar());
    }

    [HttpGet]
    public IActionResult VerDetalle(int id) {
        return View(repositorioPresupuestos.Obtener(id));
    }
}