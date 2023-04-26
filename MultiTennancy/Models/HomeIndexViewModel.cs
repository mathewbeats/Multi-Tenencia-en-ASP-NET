using MultiTennancy.Entidades;

namespace MultiTennancy.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Producto> Productos = new List<Producto>();   

        public IEnumerable<Pais> Paises = new List<Pais>(); 

    }
}
