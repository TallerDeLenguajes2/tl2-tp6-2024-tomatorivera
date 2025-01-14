using Microsoft.AspNetCore.Mvc;
using Models;
using Persistence;
using tl2_tp6_2024_tomatorivera.ViewModels;

namespace tl2_tp6_2024_tomatorivera.Controllers;

public class ProductoController : Controller
{
    private readonly ILogger<ProductoController> _logger;
    private readonly IRepository<Producto> repositorioProductos;

    public ProductoController(ILogger<ProductoController> logger, IRepository<Producto> repositorioProductos)
    {
        _logger = logger;
        this.repositorioProductos = repositorioProductos;
    }

    [HttpGet]
    [AccessLevel(AccessLevel.CLIENTE)]
    public IActionResult Listar()
    {
        return View(repositorioProductos.Listar());
    }

    [HttpGet]
    [AccessLevel(AccessLevel.CLIENTE)]
    public IActionResult Alta()
    {
        return View();
    }

    [HttpPost("api/AltaProducto")]
    [AccessLevel(AccessLevel.CLIENTE)]
    public IActionResult AltaProducto(AltaProductoViewModel productoViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("Alta");
        }

        repositorioProductos.Insertar(new Producto(productoViewModel.Descripcion, productoViewModel.Precio));
        return RedirectToAction("Listar");
    }

    [HttpGet]
    [AccessLevel(AccessLevel.CLIENTE)]
    public IActionResult Modificar(int id)
    {
        return View(repositorioProductos.Obtener(id));
    }

    [HttpPost("api/ModificarProducto")]
    [AccessLevel(AccessLevel.CLIENTE)]
    public IActionResult ModificarProducto(Producto producto)
    {
        repositorioProductos.Modificar(producto.IdProducto, producto);
        return RedirectToAction("Listar");
    }

    [HttpGet]
    [AccessLevel(AccessLevel.CLIENTE)]
    public IActionResult Eliminar(int id) {
        return View(repositorioProductos.Obtener(id));
    }

    [HttpGet("api/EliminarProducto/{id}")]
    [AccessLevel(AccessLevel.CLIENTE)]
    public IActionResult EliminarProducto(int id)
    {
        repositorioProductos.Eliminar(id);
        return RedirectToAction("Listar");
    }
}