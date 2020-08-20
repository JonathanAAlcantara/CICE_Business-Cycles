using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class Algoritmo
    {
        public static double desviaciontGlobal { get; set; }
        public List<Models.Correccion> CalcularAlgoritmo(List<Models.DataSerie> resultado)
        {
            //PROCESO UNO OBTENER LOGARITMO DE LA SERIE 

            List<Models.Logaritmo> serielog = new List<Models.Logaritmo>();

            for (int i = 0; i < resultado.Count; i++)
            {
                Models.Logaritmo logaritmo = new Models.Logaritmo();

                var actual = resultado[i];

                double log = Math.Log(Convert.ToDouble(actual.Data));

                logaritmo.datoLog = log;
                logaritmo.data = Convert.ToDouble(actual.Data);
                logaritmo.fecha = Convert.ToDateTime(actual.Date);

                serielog.Add(logaritmo);
            }

            //PROCESO DOS PRIMERAS DIFERENCIAS

            List<Models.Diferencias> seriedif = new List<Models.Diferencias>();

            bool primerElemento = true;

            for (int i = 0; i < serielog.Count; i++)
            {
                Models.Diferencias diferencias = new Models.Diferencias();

                //IDENTIFICAR EL PRIMER ELEMENTO
                if (primerElemento == true)
                {
                    primerElemento = false;

                    Models.Logaritmo lactual = serielog[i];

                    diferencias.dif = 0;
                    diferencias.datoLog = lactual.datoLog;
                    diferencias.data = Convert.ToDouble(lactual.data);
                    diferencias.fecha = Convert.ToDateTime(lactual.fecha);

                    seriedif.Add(diferencias);
                }
                else
                {
                    Models.Logaritmo lactual = serielog[i];
                    Models.Logaritmo lanterior = serielog[i - 1];

                    double odif = lactual.datoLog - lanterior.datoLog;

                    diferencias.dif = odif;
                    diferencias.datoLog = lactual.datoLog;
                    diferencias.data = Convert.ToDouble(lactual.data);
                    diferencias.fecha = Convert.ToDateTime(lactual.fecha);

                    seriedif.Add(diferencias);
                }
            }

            //PROCESO TRES OBTENER DESVIACIÓN ESTÁNDAR Y MULTIPLICARLA POR 3

            //VARIANZA Y DESVIACION TOTAL
            double Mt = 0.0;
            double St = 0.0;
            int kt = 1;

            seriedif.ForEach(n =>
            {
                double tmpM = Mt;
                Mt += (Convert.ToDouble(n.dif) - tmpM) / kt;
                St += (Convert.ToDouble(n.dif) - tmpM) * (Convert.ToDouble(n.dif) - Mt);
                kt++;
            });

            double varianzat = St / (kt - 2);
            double desviaciont = Math.Sqrt(St / (kt - 2));
            desviaciontGlobal = desviaciont;
            double factordesvest = desviaciont * 3;

            //PROCESO CUATRO VALOR DE LA SERIE EN DIFERENCIAS DIVIDIDO POR EL FACTOR "factordesvest"

            List<Models.Factorizado> seriefactorizada = new List<Models.Factorizado>();

            for (int i = 0; i < seriedif.Count; i++)
            {
                Models.Factorizado factorizado = new Models.Factorizado();

                Models.Diferencias factual = seriedif[i];

                double ofact = seriedif[i].dif / factordesvest;

                factorizado.datofactorizado = ofact;
                factorizado.dif = factual.dif;
                factorizado.datoLog = factual.datoLog;
                factorizado.data = Convert.ToDouble(factual.data);
                factorizado.fecha = Convert.ToDateTime(factual.fecha);

                seriefactorizada.Add(factorizado);
            }

            //PROCESO CINCO IDENTIFICAR DATOS ATIPICOS "n < -1 | n > 1"

            List<Models.DatoAtipico> serieconatipico = new List<Models.DatoAtipico>();

            int m;

            for (int i = 0; i < seriefactorizada.Count; i++)
            {
                Models.DatoAtipico ident = new Models.DatoAtipico();

                Models.Factorizado iactual = seriefactorizada[i];

                //IDENTIFICAR EL PRIMER ELEMENTO
                if (iactual.datofactorizado < -1 || iactual.datofactorizado > 1)
                {
                    m = 1;

                    ident.identificador = m;
                    ident.datofactorizado = iactual.datofactorizado;
                    ident.dif = iactual.dif;
                    ident.datoLog = iactual.datoLog;
                    ident.data = Convert.ToDouble(iactual.data);
                    ident.fecha = Convert.ToDateTime(iactual.fecha);

                    serieconatipico.Add(ident);
                }
                else
                {
                    m = 0;

                    ident.identificador = m;
                    ident.datofactorizado = iactual.datofactorizado;
                    ident.dif = iactual.dif;
                    ident.datoLog = iactual.datoLog;
                    ident.data = Convert.ToDouble(iactual.data);
                    ident.fecha = Convert.ToDateTime(iactual.fecha);

                    serieconatipico.Add(ident);
                }
            }

            //PROCESO SEIS CORRECIÓN CON INTERPOLACIÓN INMEDIATA (anterior y consecuente)

            List<Models.Correccion> seriecorregida = new List<Models.Correccion>();

            double c = 0.0;

            for (int i = 0; i < serieconatipico.Count; i++)
            {
                Models.Correccion corr = new Models.Correccion();

                Models.DatoAtipico corractual = serieconatipico[i];

                //IDENTIFICAR EL PRIMER ELEMENTO
                if (corractual.identificador == 1 && i != serieconatipico.Count - 1)
                {
                    Models.DatoAtipico datot1 = serieconatipico[i + 1];
                    Models.DatoAtipico datot2 = serieconatipico[i - 1];

                    c = (datot1.data + datot2.data) / 2;

                    corr.datocorregido = c;
                    corr.datoLog = corractual.datoLog;
                    corr.dif = corractual.dif;
                    corr.datofactorizado = corractual.datofactorizado;
                    corr.identificador = corractual.identificador;
                    corr.data = corractual.data;
                    corr.fecha = corractual.fecha;

                    seriecorregida.Add(corr);
                }
                else
                {
                    corr.datocorregido = corractual.data;
                    corr.datoLog = corractual.datoLog;
                    corr.dif = corractual.dif;
                    corr.datofactorizado = corractual.datofactorizado;
                    corr.identificador = corractual.identificador;
                    corr.data = corractual.data;
                    corr.fecha = corractual.fecha;

                    seriecorregida.Add(corr);
                }
            }

            return seriecorregida;
        }
    }
}




