namespace ManejoPresupuesto.Servicios
{
    public interface IServicioUsuarios
    {
        int ObtenerUsuariosId();
    }

    public class ServicioUsuarios : IServicioUsuarios
    {
        public int ObtenerUsuariosId()
        {
            return 1;
        }
    }
}
