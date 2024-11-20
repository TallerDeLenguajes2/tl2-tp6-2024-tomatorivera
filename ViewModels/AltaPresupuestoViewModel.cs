using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

namespace tl2_tp6_2024_tomatorivera.ViewModels
{
    public class AltaPresupuestoViewModel
    {
        private string _fechaCreacion;
        private int _idCliente;
        private IEnumerable<SelectListItem> _clientes;

        public AltaPresupuestoViewModel(IEnumerable<SelectListItem> clientes)
        {
            _fechaCreacion = string.Empty;
            _idCliente = 0;
            _clientes = clientes;
        }

        public AltaPresupuestoViewModel() 
        {
            _fechaCreacion = string.Empty;
            _idCliente = 0;
            _clientes = new List<SelectListItem>();
        }

        public string FechaCreacion { get => _fechaCreacion; set => _fechaCreacion = value; }
        public IEnumerable<SelectListItem> Clientes { get => _clientes; set => _clientes = value; }
        public int IdCliente { get => _idCliente; set => _idCliente = value; }

        public AltaPresupuestoViewModel MapearClientes(List<Cliente> clientes) 
        {
            _clientes = clientes.Select(c => new SelectListItem()
                                        {
                                            Value = c.Id.ToString(),
                                            Text = c.Nombre
                                        });
            return this;
        }
    }
}