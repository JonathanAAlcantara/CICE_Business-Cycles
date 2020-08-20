using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Runtime.Serialization.Json;

namespace Controllers
{
    public class APIBanxicoController
    {
        public List<DataSerie> ReadSerie(string seriesID, string fechainicio, string fechafinal)
        {
            Response resBan = new Response();
            List<DataSerie> resultado = new List<DataSerie>();

            try
            {
                string url = "https://www.banxico.org.mx/SieAPIRest/service/v1/series/" + seriesID + "/datos/" + fechainicio + "/" + fechafinal;

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //Se crea una cadena con los valores que se requiera consumir
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Accept = "application/json; charset=utf-8";
                request.Headers["Bmx-Token"] = "6af6e6645653ed1cb3ecb5165c3d30df2d5289600811f7067b2169c9ff030eb4";
                //request.Method = "GET";
                request.PreAuthenticate = true;

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                    "Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));

                //De esta forma se obtiene el JSON de la respuesta en una cadena. Esta cadena puede ser mapeada a objetos de la siguiente forma:
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));


                object c = jsonSerializer.ReadObject(response.GetResponseStream());

                resBan = (Response)c;

                var uno1 = resBan.seriesResponse.series[0];

                
                resultado = uno1.Data.ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return resultado;


        }


    }
}
