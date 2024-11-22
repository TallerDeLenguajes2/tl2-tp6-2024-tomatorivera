using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Persistence;
using tl2_tp6_2024_tomatorivera.ViewModels;

namespace tl2_tp6_2024_tomatorivera.Controllers;

public class PresupuestoController : Controller
{
    private readonly ILogger<PresupuestoController> _logger;
    private readonly IPresupuestoRepository repositorioPresupuestos;
    private readonly IRepository<Producto> repositorioProductos;
    private readonly IRepository<Cliente> repositorioClientes;

    public PresupuestoController(ILogger<PresupuestoController> logger, IPresupuestoRepository repositorioPresupuestos, IRepository<Producto> repositorioProductos, IRepository<Cliente> repositorioClientes)
    {
        _logger = logger;
        this.repositorioPresupuestos = repositorioPresupuestos;
        this.repositorioProductos = repositorioProductos;
        this.repositorioClientes = repositorioClientes;
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
        var productos = repositorioProductos.Listar()
                                            .Select(p => new SelectListItem()
                                                    {
                                                        Value = p.IdProducto.ToString(),
                                                        Text = p.Descripcion
                                                    }
                                            );
        return View(new AltaProductoDetalleViewModel(id, productos));
    }

    [HttpPost]
    public IActionResult AgregarProductoDetalle(AltaProductoDetalleViewModel datos) {
        repositorioPresupuestos.InsertarDetalle(datos.IdPresupuesto, datos.IdProducto, datos.Cantidad);
        return RedirectToAction("Listar");
    }

    [HttpGet]
    public IActionResult AltaPresupuesto() {
        var clientes = repositorioClientes.Listar()
                                          .Select(c => new SelectListItem()
                                                {
                                                    Value = c.Id.ToString(),
                                                    Text = c.Nombre
                                                });
        return View(new AltaPresupuestoViewModel(clientes));
    }

    [HttpPost]
    public IActionResult AltaPresupuesto(AltaPresupuestoViewModel presupuestoViewModel) {
        var cliente = repositorioClientes.Obtener(presupuestoViewModel.IdCliente);
        if (cliente.Id == -1)
        {
            ModelState.AddModelError("Clientes", "El cliente seleccionado no existe");
        }
        
        if (!ModelState.IsValid)
        {
            return View(presupuestoViewModel.MapearClientes(repositorioClientes.Listar()));
        }

        repositorioPresupuestos.Insertar(new Presupuesto(cliente, presupuestoViewModel.FechaCreacion));
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

    [HttpGet]
    public IActionResult Eliminar(int id) {
        return View(repositorioPresupuestos.Obtener(id));
    }

    [HttpGet]
    public IActionResult EliminarPresupuesto(int Id) {
        repositorioPresupuestos.Eliminar(Id);
        return RedirectToAction("Listar");
    }
}