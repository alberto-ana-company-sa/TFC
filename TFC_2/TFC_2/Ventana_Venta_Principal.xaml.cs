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
    /// Lógica de interacción para Ventana_Venta_Principal.xaml
    /// </summary>
    public partial class Ventana_Venta_Principal : Window
    {
        public static Ventana_Venta_Principal v;

        public Ventana_Venta_Principal()
        {
            InitializeComponent();
            Ventana_Venta_Principal.v = this;


            dateFecha.SelectedDate = System.DateTime.Now;
            cb_TipoPedido.SelectedIndex = 0;


            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaPresupuestoCompra = new MySqlCommand("SELECT * FROM empleados WHERE Codigo = " + txtVendedor.Text, conexionBBDD); ;
                reader = consultaPresupuestoCompra.ExecuteReader();

                reader.Read();
                lblNombreVendedor.Content = reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3);
                
                reader.Close();
                conexionBBDD.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static string codigoDireccion = "";
        public static string codigoTelefono = "";
        public static string codigoEmail = "";
       

        private void txt_CodigoPedido_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string num = "";
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    conexionBBDD.Open();
                    MySqlDataReader reader = null;
                    MySqlCommand consultaPresupuestoCompra = new MySqlCommand("SELECT * FROM pedido_venta", conexionBBDD);
                    reader = consultaPresupuestoCompra.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        num = reader.GetString(0);
                    }
                    if (num != "" && num != null)
                    {
                        txt_CodigoPedido.Text = num;
                    }
                    else
                    {
                        txt_CodigoPedido.Text = "1";
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

        private void btnBuscarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Busqueda ventana_Busqueda = new Ventana_Busqueda("Empleados");
            ventana_Busqueda.Show();
        }

        private void btnBuscarCodigoCliente_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Busqueda ventana_Busqueda = new Ventana_Busqueda("Clientes");
            ventana_Busqueda.Show();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            //UPDATE DE COMENTARIO EN LA TABLA DE PEDIDOS
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //UPDATE DE COMENTARIO EN LA TABLA DE PEDIDOS
        }
    }
}
