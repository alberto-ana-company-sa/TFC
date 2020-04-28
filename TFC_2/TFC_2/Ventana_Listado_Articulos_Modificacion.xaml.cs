using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TFC_2
{
    /// <summary>
    /// Lógica de interacción para Ventana_Listado_Articulos_Modificacion.xaml
    /// </summary>
    public partial class Ventana_Listado_Articulos_Modificacion : Window
    {

        public Ventana_Listado_Articulos_Modificacion()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        String codigo = "";

        public void datosSelccionadosProducto(String codigo)
        {
           this.codigo = codigo;
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaProducto = new MySqlCommand("SELECT SUM(lineas_factura_compras.Precio) FROM lineas_factura_compras", conexionBBDD);
                reader = consultaProducto.ExecuteReader();
                reader.Read();
                if (!reader.IsDBNull(0))
                {
                   TB
                }
                else
                {
                   
                }
                reader.Close();
                conexionBBDD.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
