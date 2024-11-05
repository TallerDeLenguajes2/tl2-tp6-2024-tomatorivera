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

    [HttpGet]
    public IActionResult Listar()
    {
        return View(repositorioProductos.Listar());
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

    [HttpPost("api/ModificarProducto")]
    public IActionResult ModificarProducto(Producto producto)
    {
        repositorioProductos.Modificar(producto.IdProducto, producto);
        return RedirectToAction("Listar");
    }

    [HttpGet]
    public IActionResult Eliminar(int id) {
        return View(repositorioProductos.Obtener(id));
    }

    [HttpGet("api/EliminarProducto/{id}")]
    public IActionResult EliminarProducto(int id)
    {
        repositorioProductos.Eliminar(id);
        return RedirectToAction("Listar");
    }
}