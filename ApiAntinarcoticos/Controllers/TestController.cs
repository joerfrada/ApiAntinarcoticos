using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Entidades;

namespace ApiAntinarcoticos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private TipoAmbiente tipoAmbiente = AmbienteEntity.GetTipoAmbiente();

        [HttpGet("GetTest")]
        public MensajeEntity GetTest()
        {
            MensajeEntity msg = new MensajeEntity();
            msg.Mensaje = "Ambiente: " + tipoAmbiente.ToString();

            return msg;
        }
    }
}
