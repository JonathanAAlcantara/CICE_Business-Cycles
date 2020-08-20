using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class Nodo
    {
        public string nodoPadre;
        public string nodo;
        public string desc;

        public Nodo(string _nodoPadre, string _nodo, string _desc)
        {
            nodoPadre = _nodoPadre;
            nodo = _nodo;
            desc = _desc;
        }

    }
}
