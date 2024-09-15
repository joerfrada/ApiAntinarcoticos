using System;
using System.Collections.Generic;
using Core.Entidades;

namespace Core.Servicios
{
    public interface IParametroSistemaManager
    {
        public ParametroSistemaEntity GetParametros(string pCodigo);
    }
}
