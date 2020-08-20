using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [DataContract]
    public class SeriesResponse
    {
        [DataMember(Name = "series")]
        public Serie[] series { get; set; }
    }
}
