using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class DetectorTrimController
    {
        //MÉTODO PARA DETECTAR LOS PUNTOS DE QUIEBRE DE UNA SERIE, ACEPTA DOS SOBRECARGAS
        //DetectordeQuiebres(List<Models.MediaMovil> parámetro)
        //DetectordeQuiebres(List<Models.DataSerie> parámetro)
        //-----SE OBTIENEN LOS PUNTOS MÁXIMOS Y MÍNIMOS DE ENTRE UN GRUPO DE +-12 VALORES-----

        public List<Models.Detector> DetectordeQuiebresTrimestral(List<Models.MediaMovil> seriemediamovil)
        {
            //PROCESO PARA DETECTAR LOS PUNTOS DE QUIEBRE
            List<Models.Detector> seriemediamovilminmax = new List<Models.Detector>();
            List<Models.Detector> seriemediamovilminmaxtot = new List<Models.Detector>();

            for (int i = 4; i < ((seriemediamovil.Count) - 4); i++)
            {
                seriemediamovilminmax = new List<Models.Detector>();
                Models.Detector detector = new Models.Detector();

                for (int x = 4; x > 0; x--)
                {
                    detector = new Models.Detector();
                    detector.mediamovil = seriemediamovil[i - x].mediamovil;
                    detector.fecha = seriemediamovil[i - x].fecha;
                    detector.indice = i - x;

                    seriemediamovilminmax.Add(detector);
                }

                detector = new Models.Detector();
                detector.mediamovil = seriemediamovil[i].mediamovil;
                detector.fecha = seriemediamovil[i].fecha;
                detector.indice = i;

                seriemediamovilminmax.Add(detector);
                seriemediamovilminmaxtot.Add(detector);

                for (int x = 1; x < 5; x++)
                {
                    detector = new Models.Detector();
                    detector.mediamovil = seriemediamovil[i + x].mediamovil;
                    detector.fecha = seriemediamovil[i + x].fecha;
                    detector.indice = i + x;

                    seriemediamovilminmax.Add(detector);
                }



                double? maximo = seriemediamovilminmax.Max(z => z.mediamovil).Value;

                if(maximo != null)
                {
                    if(seriemediamovil[i].mediamovil == maximo)
                    {
                        seriemediamovilminmaxtot.Last().detectormax = 1;
                    }
                }

                double? minimo = seriemediamovilminmax.Min(z => z.mediamovil).Value;

                if (minimo != null)
                {
                    if (seriemediamovil[i].mediamovil == minimo)
                    {
                        seriemediamovilminmaxtot.Last().detectormin = -1;
                    }
                }

            }
            return seriemediamovilminmaxtot;
        }

        public List<Models.Detector> DetectordeQuiebresTrimestral(List<Models.DataSerie> resultado)
        {
            //PROCESO PARA DETECTAR LOS PUNTOS DE QUIEBRE EN 
            List<Models.Detector> seriemediamovilminmax = new List<Models.Detector>();
            List<Models.Detector> seriemediamovilminmaxtot = new List<Models.Detector>();

            for (int i = 4; i < ((resultado.Count) - 4); i++)
            {
                seriemediamovilminmax = new List<Models.Detector>();
                Models.Detector detector = new Models.Detector();

                for (int x = 4; x > 0; x--)
                {
                    detector = new Models.Detector();
                    detector.mediamovil = Convert.ToDouble(resultado[i - x].Data);
                    detector.fecha = Convert.ToDateTime(resultado[i - x].Date);
                    detector.indice = i - x;

                    seriemediamovilminmax.Add(detector);
                }

                detector = new Models.Detector();
                detector.mediamovil = Convert.ToDouble(resultado[i].Data);
                detector.fecha = Convert.ToDateTime(resultado[i].Date);
                detector.indice = i;

                seriemediamovilminmax.Add(detector);
                seriemediamovilminmaxtot.Add(detector);

                for (int x = 1; x < 5; x++)
                {
                    detector = new Models.Detector();
                    detector.mediamovil = Convert.ToDouble(resultado[i + x].Data);
                    detector.fecha = Convert.ToDateTime(resultado[i + x].Date);
                    detector.indice = i + x;

                    seriemediamovilminmax.Add(detector);
                }



                double? maximo = seriemediamovilminmax.Max(z => z.mediamovil).Value;

                if (maximo != null)
                {
                    if (Convert.ToDouble(resultado[i].Data) == maximo)
                    {
                        seriemediamovilminmaxtot.Last().detectormax = 1;
                    }
                }

                double? minimo = seriemediamovilminmax.Min(z => z.mediamovil).Value;

                if (minimo != null)
                {
                    if (Convert.ToDouble(resultado[i].Data) == minimo)
                    {
                        seriemediamovilminmaxtot.Last().detectormin = -1;
                    }
                }

            }
            return seriemediamovilminmaxtot;

        }
    }
}
