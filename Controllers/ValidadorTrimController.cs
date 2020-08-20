using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class ValidadorTrimController
    {
        //Método que valida si los mínimos y máximos detectados son puntos de quiebre
        //bajo tres criterios y calcular estadísticos
        public List<Models.Salida> ValidadordeSalidaTrimestral(List<Models.Detector> serieorgininalquiebres, List<Models.Detector> seriecorregidaquiebres, List<Models.DataSerie> resultado)
        {
            //CRITERIO 1
            foreach (Models.Detector det in serieorgininalquiebres.Where(n => n.detectormax == 1))
            {
                int indice = det.indice;

                if (indice >= 6)
                {
                    int total = serieorgininalquiebres.Where(n => n.indice >= (indice - 5) && n.indice <= (indice + 5)).Sum(m => m.detectormax);

                    if (total == 1)
                    {
                        det.criterioMax1 = 1;
                        det.fase = "Pico";
                    }
                    else
                    {
                        det.criterioMax1 = 2;
                        det.fase = "Pico";
                    }
                }
            }

            foreach (Models.Detector det in serieorgininalquiebres.Where(n => n.detectormin == -1))
            {
                int indice = det.indice;

                if (indice >= 6)
                {
                    int total = serieorgininalquiebres.Where(n => n.indice >= (indice - 5) && n.indice <= (indice + 5)).Sum(m => m.detectormin);

                    if (total == -1)
                    {
                        det.criterioMin1 = 1;
                        det.fase = "Valle";
                    }
                    else
                    {
                        det.criterioMin1 = 2;
                        det.fase = "Valle";
                    }
                }
            }

            //CRITERIO 2
            //foreach (Models.Detector d in seriecorregidaquiebres)
            //{
            //    d.indice = d.indice + 3;
            //}

            //for (int w = 0; w < 6; w++)
            //{
            //    Models.Detector detectorIni = new Models.Detector();
            //    detectorIni.mediamovil = null;
            //    detectorIni.indice = w;
            //    detectorIni.detectormax = 0;
            //    detectorIni.detectormin = 0;
            //    seriecorregidaquiebres.Insert(w, detectorIni);
            //}

            foreach (Models.Detector det in serieorgininalquiebres.Where(n => n.detectormax == 1 && n.criterioMax1 >= 1))
            {
                int indice = det.indice;

                if (indice >= 3)
                {
                    var criterio2 = seriecorregidaquiebres.Where(n => (n.indice >= (indice - 2) && n.indice <= (indice + 2)) && n.detectormax == 1).ToList();

                    if (criterio2.Count > 0)
                    {
                        det.criterio2 = 1;
                    }
                    else
                    {
                        det.criterio2 = 0;
                    }
                }
            }

            foreach (Models.Detector det in serieorgininalquiebres.Where(n => n.detectormin == -1 && n.criterioMin1 >= 1))
            {
                int indice = det.indice;

                if (indice >= 3)
                {
                    var criterio2 = seriecorregidaquiebres.Where(n => (n.indice >= (indice - 2) && n.indice <= (indice + 2)) && n.detectormin == -1).ToList();

                    if (criterio2.Count > 0)
                    {
                        det.criterio2 = 1;
                    }
                    else
                    {
                        det.criterio2 = 0;
                    }
                }
            }


            //CRITERIO 3
            var listaCriterio3 = serieorgininalquiebres.Where(n => (n.detectormax == 1 && n.criterioMax1 >= 1 && n.criterio2 == 1) || (n.detectormin == -1 && n.criterioMin1 >= 1 && n.criterio2 == 1)).ToList();
            bool primerElemento = true;

            Models.Detector tipoAnt = new Models.Detector();
            Models.Detector tipo = new Models.Detector();

            List<Models.Salida> salida = new List<Models.Salida>();

            foreach (Models.Detector det3 in listaCriterio3)
            {
                Models.Salida osalida = new Models.Salida();

                double? resCriterio3 = 0;
                double? crecMensual = 0;

                tipo = det3;
                if (primerElemento)
                {
                    tipoAnt = det3;
                    primerElemento = false;

                    resCriterio3 = (tipo.mediamovil / serieorgininalquiebres[0].mediamovil) - 1;
                    crecMensual = (((tipo.mediamovil / serieorgininalquiebres[0].mediamovil) - 1) / (tipo.indice - serieorgininalquiebres[0].indice)) * 100;


                    if (resCriterio3 > (Algoritmo.desviaciontGlobal))
                    {
                        det3.criterio3 = 1;

                    }
                    else
                    {
                        det3.criterio3 = 0;
                    }


                    osalida.cambioTotal = resCriterio3 * 100;
                    osalida.cambioMensual = crecMensual;
                    osalida.indice = det3.indice;
                    osalida.fecha = det3.fecha;
                    osalida.fase = det3.fase;
                    salida.Add(osalida);

                }
                else
                {
                    if (det3.detectormax == 1)
                    {
                        tipo = det3;
                        if (tipoAnt.detectormin == -1)
                        {
                            resCriterio3 = (tipo.mediamovil / tipoAnt.mediamovil) - 1;
                            crecMensual = (((tipo.mediamovil / tipoAnt.mediamovil) - 1) / (tipo.indice - tipoAnt.indice)) * 100;

                            if (resCriterio3 > (Algoritmo.desviaciontGlobal))
                            {
                                det3.criterio3 = 1;

                            }
                            else
                            {
                                det3.criterio3 = 0;
                            }

                        }
                        osalida.cambioTotal = resCriterio3 * 100;
                        osalida.cambioMensual = crecMensual;
                        osalida.indice = det3.indice;
                        osalida.fecha = det3.fecha;
                        osalida.fase = det3.fase;
                        salida.Add(osalida);
                        tipoAnt = det3;
                    }
                    else if (det3.detectormin == -1)
                    {
                        tipo = det3;
                        if (tipoAnt.detectormax == 1)
                        {
                            resCriterio3 = (tipo.mediamovil / tipoAnt.mediamovil) - 1;
                            crecMensual = (((tipo.mediamovil / tipoAnt.mediamovil) - 1) / (tipo.indice - tipoAnt.indice)) * 100;

                            if (resCriterio3 < (Algoritmo.desviaciontGlobal * -1))
                            {
                                det3.criterio3 = 2;
                            }
                            else
                            {
                                det3.criterio3 = 0;
                            }
                        }

                        osalida.cambioTotal = resCriterio3;
                        osalida.cambioMensual = crecMensual;
                        osalida.indice = det3.indice;
                        osalida.fecha = det3.fecha;
                        osalida.fase = det3.fase;
                        salida.Add(osalida);
                        tipoAnt = det3;
                    }

                }
            }

            //CALCULAMOS LAS ESTADÍSTICAS PATRA IMPRIMIR EN LA TABLA

            //CALCULAMOS LA VARIANZA Y DURACIÓN DE LA FASE 

            List<Models.Salida> salidavar = new List<Models.Salida>();

            for (int k = 0; k < listaCriterio3.Count; k++)
            {
                if (k == 0)
                {
                    Models.Detector finalv = listaCriterio3[k];
                    Models.Detector actualv = new Models.Detector();
                    actualv.indice = 0;
                    Models.Salida var = new Models.Salida();

                    double? sumavar = 0;
                    int observaciones = 0;
                    double? media = 0;
                    double? varianza = 0;
                    double? ovarianza = 0;

                    for (int l = actualv.indice; l < finalv.indice; l++)
                    {
                        sumavar += Convert.ToDouble(resultado[l].Data);
                        observaciones = (finalv.indice - actualv.indice);
                        media = sumavar / observaciones;
                    }

                    for (int l = actualv.indice; l < finalv.indice; l++)
                    {
                        ovarianza += (Convert.ToDouble(resultado[l].Data) - media) * (Convert.ToDouble(resultado[l].Data) - media);
                        varianza = ovarianza / (observaciones - 1);
                    }

                    var.varianza = varianza;
                    var.duracion = observaciones;
                    var.fecha = listaCriterio3[k].fecha;

                    salidavar.Add(var);
                }
                else
                {
                    Models.Detector finalv = listaCriterio3[k];
                    Models.Detector actualv = listaCriterio3[k - 1];
                    Models.Salida var = new Models.Salida();

                    double? sumavar = 0;
                    int observaciones = 0;
                    double? media = 0;
                    double? varianza = 0;
                    double? ovarianza = 0;

                    for (int l = actualv.indice; l < finalv.indice; l++)
                    {
                        sumavar += Convert.ToDouble(resultado[l].Data);
                        observaciones = (finalv.indice - actualv.indice);
                        media = sumavar / observaciones;
                    }

                    for (int l = actualv.indice; l < finalv.indice; l++)
                    {
                        ovarianza += (Convert.ToDouble(resultado[l].Data) - media) * (Convert.ToDouble(resultado[l].Data) - media);
                        varianza = ovarianza / (observaciones - 1);
                    }

                    var.varianza = varianza;
                    var.duracion = observaciones;
                    var.fecha = listaCriterio3[k].fecha;

                    salidavar.Add(var);
                }
            }


            List<Models.Salida> resumen = new List<Models.Salida>();

            for (int i = 0; i < salidavar.Count; i++)
            {
                Models.Salida r = new Models.Salida();

                r.cambioMensual = salida[i].cambioMensual;
                r.cambioTotal = salida[i].cambioTotal;
                r.duracion = salidavar[i].duracion;
                r.fecha = salida[i].fecha;
                r.varianza = salidavar[i].varianza;
                r.fase = salida[i].fase;
                r.indice = salida[i].indice;

                resumen.Add(r);
            }
            return resumen;
        }
    }
}
