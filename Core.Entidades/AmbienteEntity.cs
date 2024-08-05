using System;

namespace Core.Entidades
{
    public enum TipoAmbiente { Desarrollo, Produccion, Pruebas };

    public static class AmbienteEntity
    {
        public static TipoAmbiente GetTipoAmbiente()
        {
#if DEBUG_DESARROLLO || DESARROLLO
            return TipoAmbiente.Desarrollo;
#elif DEBUG_PRODUCCION || PRODUCCION
            return TipoAmbiente.Produccion;
#elif DEBUG_PRUEBAS || PRUEBAS
            return TipoAmbiente.Pruebas;
#endif
        }
    }
}
