using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Detector
    {
        public double? mediamovil { get; set; }
        public DateTime fecha { get; set; }
        public int detectormax { get; set; }
        public int detectormin { get; set; }
        public int indice { get; set; }
        public int criterioMax1 { get; set; }
        public int criterioMin1 { get; set; }
        public int criterio2 { get; set; }
        public int criterio3 { get; set; }
        public int punto { get; set; }
        public string fase { get; set; }
    }
}
