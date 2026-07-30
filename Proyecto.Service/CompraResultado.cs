namespace Proyecto.Service
{
    public class CompraResultado
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public int? OrdenId { get; set; }

        public static CompraResultado Ok(int ordenId)
        {
            return new CompraResultado
            {
                Exitoso = true,
                OrdenId = ordenId,
                Mensaje = "Compra realizada correctamente."
            };
        }

        public static CompraResultado Error(string mensaje)
        {
            return new CompraResultado
            {
                Exitoso = false,
                Mensaje = mensaje
            };
        }
    }
}
