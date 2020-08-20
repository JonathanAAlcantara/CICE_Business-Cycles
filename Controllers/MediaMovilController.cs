using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class MediaMovilController
    {
        public List<Models.MediaMovil> CalcularMediaMovil(List<Models.Correccion> seriecorregida3)
        {
            //PROCESO PARA OBTENER LA MEDIA MOVIL DE LA SERIE

            List<Models.MediaMovil> seriemediamovil = new List<Models.MediaMovil>();

            for (int i = 3; i < ((seriecorregida3.Count)-3); i++) 
            {
                Models.MediaMovil mediamovil = new Models.MediaMovil();

                Models.Correccion mm0 = seriecorregida3[i];
                Models.Correccion mm_1 = seriecorregida3[i - 1];
                Models.Correccion mm_2 = seriecorregida3[i - 2];
                Models.Correccion mm_3 = seriecorregida3[i - 3];
                Models.Correccion mm1 = seriecorregida3[i + 1];
                Models.Correccion mm2 = seriecorregida3[i + 2];
                Models.Correccion mm3 = seriecorregida3[i + 3];

                double mm = ((mm0.datocorregido + mm_1.datocorregido + mm_2.datocorregido + mm_3.datocorregido + mm1.datocorregido + mm2.datocorregido + mm3.datocorregido) / 7);

                mediamovil.mediamovil = mm;
                mediamovil.fecha = mm0.fecha;

                seriemediamovil.Add(mediamovil);
            }
            return seriemediamovil;
        }
    }
}
