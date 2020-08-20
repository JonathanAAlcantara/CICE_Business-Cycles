using Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Controllers
{
    public class APIInegiController
    {

        public List<Models.DataSerie> ReadSerieINEGI(string SerieID, string fechainicio, string fechafin)
        {
            DataSerie resBan = new DataSerie();
            List<Models.DataSerie> oresultado = new List<Models.DataSerie>();
            List<Models.DataSerie> Resultado = new List<Models.DataSerie>();

            string Token = "d33048f2-1ff9-089d-d7d5-eb5a00dbf702"; 

            try
            {

                string responseText = string.Empty;
                try
                {
                    string url = "https://www.inegi.org.mx";

                    string _url = url;
                    string _token = Token;
                    var client = new RestClient(string.Format("{0}/app/api/indicadores/desarrolladores/jsonxml/INDICATOR/" + SerieID + "/es/00/false/BIE/2.0/" + _token, _url));
                    //client.AddDefaultHeader("Authorization", _token);
                    var request = new RestRequest(Method.GET);
                    request.RequestFormat = DataFormat.Json;
                    IRestResponse response = client.Execute(request);
                    Newtonsoft.Json.Linq.JToken data = Newtonsoft.Json.Linq.JToken.Parse(response.Content);

                    List<Models.DataSerie> resultado = new List<Models.DataSerie>();
                    Models.DataSerie dato = new DataSerie();

                    if (data.Last.Values().Count() > 0)
                    {
                        if (data.Last.Values().ToList().Count() > 0)
                        {
                            if (data.Last.Values().ToList()[0].Last.ToList().Count() > 0)
                            {
                                foreach (var d in data.Last.Values().ToList()[0].Last.ToList()[0].ToList())
                                {
                                    var datos = JObject.Parse(d.ToString());

                                    var uno = datos.Values();

                                    dato = new DataSerie();

                                    dato.Date = uno.ToList()[0].ToString();
                                    dato.Data = uno.ToList()[1].ToString();

                                    resultado.Add(dato);
                                }
                            }
                        }
                    }

                    DateTime dtfechaIni = Convert.ToDateTime(fechainicio);
                    DateTime dtfechaIniFormato = Convert.ToDateTime(dtfechaIni.Year + "/" + dtfechaIni.Month + "/" + dtfechaIni.Day + " 00:00");

                    DateTime dtfechafin = Convert.ToDateTime(fechafin);
                    DateTime dtfechaFinFormato = Convert.ToDateTime(dtfechafin.Year + "/" + dtfechafin.Month + "/" + dtfechafin.Day + " 23:59");

                    oresultado = resultado.Where(n => Convert.ToDateTime(n.Date) >= dtfechaIniFormato && Convert.ToDateTime(n.Date) <= dtfechaFinFormato).ToList();

                    Resultado = (from n in oresultado
                                 orderby n.Date ascending
                                 select n).ToList();
                }


                catch (WebException wex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("ERROR:" + wex.Message + ". STATUS: " + wex.Status.ToString());

                    if (wex.Status == WebExceptionStatus.ProtocolError)
                    {
                        var response = ((HttpWebResponse)wex.Response);
                        sb.AppendLine(string.Format("Status Code : {0}", response.StatusCode));
                        sb.AppendLine(string.Format("Status Description : {0}", response.StatusDescription));

                        try
                        {
                            StreamReader reader = new StreamReader(response.GetResponseStream());
                            sb.AppendLine(reader.ReadToEnd());
                        }
                        catch (WebException ex) { throw; }
                    }

                    throw new Exception(sb.ToString(), wex);
                }
                catch (Exception ex) { throw; }

                //string url = "https://www.inegi.org.mx/app/api/indicadores/desarrolladores/jsonxml/INDICATOR/" + "493621" + "/es/00/false/BIE/2.0/" + Token + "?type=json";


                //WebRequest request = WebRequest.Create(url);
                //WebResponse ws = request.GetResponse();
                //JsonSerializer jsonSerializer = new JsonSerializer(typeof(DataListInegi));
                //PanoramioData photos = (PanoramioData)jsonSerializer.Deserialize(ws.GetResponseStream());

                /*HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                    "Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));

                //De esta forma se obtiene el JSON de la respuesta en una cadena. Esta cadena puede ser mapeada a objetos de la siguiente forma:
                */
                //DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(DataListInegi));

                
                //object c = jsonSerializer.ReadObject(ws.GetResponseStream());
                /*
                resBan = (DataListInegi)c;

                Respuesta.Add(resBan);*/
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Resultado;
        }

    }
}
