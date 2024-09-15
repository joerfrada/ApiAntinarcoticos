using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Entidades;
using Core.Negocios;
using Core.Servicios;

namespace ApiAntinarcoticos.Controllers.Seguridad
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticaController : ControllerBase
    {
        private readonly IDataAccess dataAccess;
        private ILoginManager lgMgr;

        public AutenticaController(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        [HttpPost("Login")]
        public MensajeEntity Login([FromBody] LoginEntity request)
        {
            dataAccess.setUsuario(request.Usuario.ToUpper());
            lgMgr = new LoginManager(dataAccess);

            return lgMgr.Login(request.Usuario, request.Password);
        }
    }
}
