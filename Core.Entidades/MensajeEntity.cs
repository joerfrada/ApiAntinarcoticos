using System;

namespace Core.Entidades
{
    public enum TipoMensaje { ERROR = -1, SUCCESS = 0, WARNING = 2 }

    public class MensajeEntity
    {
        public dynamic Result { get; set; }
        public String Mensaje { get; set; }
        public TipoMensaje Tipo { get; set; }
        public Int64 Id { get; set; }
        public Int64 Total { get; set; }
    }
}
