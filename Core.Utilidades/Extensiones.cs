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
            var user = ((DefaultHttpContext)http).Request?.Headers["USER"].ToString();

            return user;
        }        

        public static bool IsNull(this object objeto)
        {
            return objeto == null || objeto is DBNull;
        }        

        public static bool IsNotNull(this object objeto)
        {
            return !objeto.IsNull();
        }

        public static bool IsNullOrEmpty(this string cadena)
        {
            return string.IsNullOrEmpty(cadena);
        }

        public static bool IsCero(this int numero)
        {
            return numero == 0;
        }

        public static bool IsValid(this DateTime fecha)
        {
            return !fecha.IsNull() && fecha > DateTime.MinValue && fecha < DateTime.MaxValue;
        }        

        public static bool IsValidaDate(this string date)
        {
            DateTime result;
            return DateTime.TryParse(date, out result);
        }

        public static bool IsNumber(this string date)
        {
            Int64 result;
            return Int64.TryParse(date, out result);
        }
    }
}
