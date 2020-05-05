using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFC_2
{
    class VentaGrafica
    {
        public string anio { get; set; }
        public string mes { get; set; }
        public double numeroVentas { get; set; }

        public VentaGrafica(string anio, string mes, double num)
        {
            this.anio = anio;
            this.mes = mes;
            this.numeroVentas = num;
        }

        public string mostrar()
        {
            return anio + " " + mes + " " + numeroVentas;
        }
    }

    class VentasPorAnio
    {
        public List<VentaGrafica> meses = new List<VentaGrafica>();
        public string anio { get; set; }

        public VentasPorAnio(string anio)
        {
            this.anio = anio;
        }
        public VentasPorAnio(string anio, List<VentaGrafica> meses)
        {
            this.anio = anio;
            this.meses = meses;
        }

        public VentasPorAnio() { }
    }
}
