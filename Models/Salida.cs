using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Salida
    {
        public double? cambioMensual { get; set; }
        public double? cambioTotal { get; set; }
        public double? varianza { get; set; }
        public int duracion { get; set; }
        public DateTime fecha { get; set; }
        public int indice { get; set; }
        public string fase { get; set; }

    }
}
