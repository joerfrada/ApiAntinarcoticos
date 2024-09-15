using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Entidades;
using Core.Negocios;
using Core.Servicios;
using Core.Utilidades;

namespace ApiAntinarcoticos.Controllers.Parametrizacion
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValorFlexibleController : ControllerBase
    {
        private readonly IDataAccess dataAccess;
        private IValorFlexibleManager vfMgr;

        public ValorFlexibleController(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        [HttpPost("GetTiposValores")]
        public MensajeEntity GetTiposValores([FromBody] RequestEntity request)
        {
            dataAccess.setUsuario(HttpContext.GetHeaderUser());
            vfMgr = new ValorFlexibleManager(dataAccess);

            long total = 0;

            List<TipoValorEntity> lstDatos = vfMgr.GetTipoValores(request, ref total);

            return new MensajeEntity()
            {
                Result = lstDatos.ToArray(),
                Total = total
            };
        }

        [HttpPost("CreateTiposValores")]
        public MensajeEntity CreateTiposValores([FromBody] TipoValorEntity request)
        {
            dataAccess.setUsuario(HttpContext.GetHeaderUser());
            vfMgr = new ValorFlexibleManager(dataAccess);

            return vfMgr.CrudTipoValores(request, "C");
        }

        [HttpPost("UpdateTiposValores")]
        public MensajeEntity UpdateTiposValores([FromBody] TipoValorEntity request)
        {
            dataAccess.setUsuario(HttpContext.GetHeaderUser());
            vfMgr = new ValorFlexibleManager(dataAccess);

            return vfMgr.CrudTipoValores(request, "U");
        }

        [HttpPost("GetValoresFlexibles")]
        public MensajeEntity GetValoresFlexibles([FromBody] RequestEntity request)
        {
            dataAccess.setUsuario(HttpContext.GetHeaderUser());
            vfMgr = new ValorFlexibleManager(dataAccess);

            long total = 0;

            List<ValorFlexibleEntity> lstDatos = vfMgr.GetValoresFlexibles(request, ref total);

            return new MensajeEntity()
            {
                Result = lstDatos.ToArray(),
                Total = total
            };
        }

        [HttpPost("CreateValoresFlexibles")]
        public MensajeEntity CreateValoresFlexibles([FromBody] ValorFlexibleEntity request)
        {
            dataAccess.setUsuario(HttpContext.GetHeaderUser());
            vfMgr = new ValorFlexibleManager(dataAccess);

            return vfMgr.CrudValoresFlexibles(request, "C");
        }

        [HttpPost("UpdateValoresFlexibles")]
        public MensajeEntity UpdateValoresFlexibles([FromBody] ValorFlexibleEntity request)
        {
            dataAccess.setUsuario(HttpContext.GetHeaderUser());
            vfMgr = new ValorFlexibleManager(dataAccess);

            return vfMgr.CrudValoresFlexibles(request, "U");
        }
    }
}
