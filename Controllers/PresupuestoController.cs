using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Persistence;

namespace tl2_tp6_2024_tomatorivera.Controllers;

public class PresupuestoController : Controller
{
    private readonly ILogger<PresupuestoController> _logger;
    private readonly IPresupuestoRepository repositorioPresupuestos;
    private readonly IRepository<Producto> repositorioProductos;

    public PresupuestoController(ILogger<PresupuestoController> logger)
    {
        _logger = logger;
        repositorioPresupuestos = new PresupuestoRepositoryImpl();
        repositorioProductos = new ProductoRepositoryImpl();
    }

    [HttpGet]
    public IActionResult Listar() {
        return View(repositorioPresupuestos.Listar());
    }

    [HttpGet]
    public IActionResult VerDetalle(int id) {
        return View(repositorioPresupuestos.Obtener(id));
    }

    [HttpGet]
    public IActionResult AgregarProductoDetalle(int id) {
        ViewData["Productos"] = repositorioProductos.Listar()
                                                    .Select(p => new SelectListItem()
                                                                {
                                                                    Value = p.IdProducto.ToString(),
                                                                    Text = p.Descripcion
                                                                });
        return View(id);
    }

    [HttpPost]
    public IActionResult AgregarProductoDetalle(int idPresupuesto, int cantidad, int producto) {
        repositorioPresupuestos.InsertarDetalle(idPresupuesto, producto, cantidad);
        return RedirectToAction("Listar");
    }

    [HttpGet]
    public IActionResult AltaPresupuesto() {
        return View();
    }

    [HttpPost]
    public IActionResult AltaPresupuesto(Presupuesto presupuesto) {
        repositorioPresupuestos.Insertar(presupuesto);
        return RedirectToAction("Listar");
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id) {
        return View(repositorioPresupuestos.Obtener(id));
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(Presupuesto presupuesto) {
        repositorioPresupuestos.Modificar(presupuesto.Id, presupuesto);
        return RedirectToAction("Listar");
    }
}