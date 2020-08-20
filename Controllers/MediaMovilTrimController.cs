using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class MediaMovilTrimController
    {
        public List<Models.MediaMovil> CalcularMediaMovilTrimestral(List<Models.Correccion> seriecorregida3)
        {
            //PROCESO PARA OBTENER LA MEDIA MOVIL DE LA SERIE

            List<Models.MediaMovil> seriemediamovil = new List<Models.MediaMovil>();

            for (int i = 1; i < ((seriecorregida3.Count) - 1); i++)
            {
                Models.MediaMovil mediamovil = new Models.MediaMovil();

                Models.Correccion mm0 = seriecorregida3[i];
                Models.Correccion mm_1 = seriecorregida3[i - 1];
                Models.Correccion mm1 = seriecorregida3[i + 1];

                double mm = ((mm0.datocorregido + mm_1.datocorregido + mm1.datocorregido) / 3);

                mediamovil.mediamovil = mm;
                mediamovil.fecha = mm0.fecha;

                seriemediamovil.Add(mediamovil);
            }
            return seriemediamovil;
        }
    }
}
