using Microsoft.AspNetCore.Mvc;
using Models;
using Persistence;

namespace tl2_tp6_2024_tomatorivera.Controllers;

public class ProductoController : Controller
{
    private readonly ILogger<ProductoController> _logger;
    private readonly IRepository<Producto> repositorioProductos;

    public ProductoController(ILogger<ProductoController> logger)
    {
        _logger = logger;
        repositorioProductos = new ProductoRepositoryImpl();
    }

    public IActionResult Operaciones()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Listar()
    {
        return View("Listar", repositorioProductos.Listar());
    }

    public IActionResult Alta()
    {
        return View();
    }

    [HttpPost("api/AltaProducto")]
    public IActionResult AltaProducto(Producto producto)
    {
        repositorioProductos.Insertar(producto);
        return RedirectToAction("Listar");
    }

    [HttpGet]
    public IActionResult Modificar(int id)
    {
        return View(repositorioProductos.Obtener(id));
    }
}