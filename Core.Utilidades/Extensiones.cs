using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Core.Entidades;

namespace Core.Utilidades
{
    public static class Extensiones
    {
        //public static TipoAmbiente tipo = AmbienteEntity.GetTipoAmbiente();

        public static string GetHeaderUser(this object http)
        {
            var headers = ((DefaultHttpContext)http).Request?.Headers;
            headers.TryGetValue("USER", out StringValues headerValue);

            return headerValue.FirstOrDefault();
        }
    }
}
