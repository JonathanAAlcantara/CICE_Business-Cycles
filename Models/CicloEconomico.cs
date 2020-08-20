using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CicloEconomico
    {
        public DataSerie actual { get; set; }
        public DataSerie anterior { get; set; }
        public Double promedio { get; set; }
        public double varianza { get; set; }
        public List<double> historico { get; set; }
        public List<DataSerie> historicoDatos { get; set; }
    }
}
