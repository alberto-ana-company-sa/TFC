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
    /// Lógica de interacción para Ventana_Proveedores_Email.xaml
    /// </summary>
    public partial class Ventana_Proveedores_Email : Window
    {
        public Ventana_Proveedores_Email()
        {
            InitializeComponent();
        }
        String ventana = "";
        String codigo = "";

        public void InformacionProveedor(string nombreCliente, string codigoCliente)
        {
            TB_Nombre_Proveedor.Text = nombreCliente;
            codigo = codigoCliente;
        }

        public void ventanaActual(string vent)
        {
            ventana = vent;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO email_proveedores (Codigo_Proveedores, Email) VALUES ('" + codigo + "' , '" + TB_Email_Proveedor.Text + "');", conexionBBDD);

                cmd.ExecuteNonQuery();

                conexionBBDD.Close();
                if (ventana.Equals("Ventana_Proveedores_Modificar_Email"))
                {
                    Ventana_Proveedores_Modificar_Email.v.DatosDataGrid();
                }
                else
                {
                    Ventana_listado_Proveedores.v.DatosDataGridProveedores();
                }

                this.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
