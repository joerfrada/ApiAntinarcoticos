using System;
using System.Collections.Generic;
using Core.Entidades;

namespace Core.Servicios
{
    public interface ILoginManager
    {
        public MensajeEntity Login(string pUsuario, string pPassword);
    }
}
