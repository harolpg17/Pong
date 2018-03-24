using Pong.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Pong.Controllers
{
    public class MensajesController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Estadistica()
        {
            EstadisticasMensajes estadisticasMensajes = new EstadisticasMensajes();
            var resultado = await estadisticasMensajes.ObtenerEstadisticas();
            return Ok(resultado);
        }
    }
}
