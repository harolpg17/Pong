using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pong.Bus
{
    public class EstadisticasMensajes
    {
        public async Task<Estadisticas> ObtenerEstadisticas()
        {
            var handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential("guest", "guest");
            var cliente = new HttpClient(handler);                
            cliente.BaseAddress = new Uri("http://localhost:15672/api/");               
            var url = "overview";
            var response = await cliente.GetAsync(url);

            var resultadoTexto = await response.Content.ReadAsStringAsync();
            JObject resultado = JObject.Parse(resultadoTexto);

            Estadisticas estadisticas = new Estadisticas
            {
                MensajesRecibidos = resultado["message_stats"]["ack"].ToString(),
                MensajesRespuestos = resultado["message_stats"]["deliver"].ToString()
            };

            return estadisticas;
        }
    }
}