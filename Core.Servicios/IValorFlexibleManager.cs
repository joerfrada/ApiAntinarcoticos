using System;
using System.Collections.Generic;
using Core.Entidades;

namespace Core.Servicios
{
    public interface IValorFlexibleManager
    {
        public MensajeEntity CrudTipoValores(TipoValorEntity request, string evento);
        public List<TipoValorEntity> GetTipoValores(RequestEntity request, ref Int64 pTotal);

        public MensajeEntity CrudValoresFlexibles(ValorFlexibleEntity request, string evento);
        public List<ValorFlexibleEntity> GetValoresFlexibles(RequestEntity request, ref Int64 pTotal);
    }
}
