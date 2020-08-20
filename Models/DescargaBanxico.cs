using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DescargaBanxico
    {
        public List<DataSerie> Serie { get; set; }

        public string IdSerie { get; set; }

        public string Nombre { get; set; }
    }
}
