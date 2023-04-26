namespace MultiTennancy.Entidades
{

    //entidad comun
    public class Pais: IEntidadComun
    {
        public int Id { get; set; }


        //Al agregar ! después de null, estás indicando al compilador que confíe en que,
        //aunque la propiedad Nombre se inicializa con null, se le asignará un valor no nulo
        //antes de ser utilizada. De esta manera, estás anulando las advertencias de nulabilidad
        //que el compilador podría emitir.
        public string Nombre { get; set; } = null!;
    }
}
 