namespace MultiTennancy.Servicios
{


    //creamos la interface IServiciotenant para obtener el Id de cada tenant
    //despues lo inyectamos en la clase ApplicationDbContext que ya ha sido 
    //creada por nosotros al aceptar auth de cuentas individuales al inciciar el proyecto

    public interface IServicioTenant
    {
        string ObtenerTenant();
    }
}
