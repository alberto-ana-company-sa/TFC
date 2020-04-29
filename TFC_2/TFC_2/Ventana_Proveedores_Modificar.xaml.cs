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
    /// Lógica de interacción para Ventana_Proveedores_Modificar.xaml
    /// </summary>
    public partial class Ventana_Proveedores_Modificar : Window
    {
        public Ventana_Proveedores_Modificar()
        {
            InitializeComponent();
        }
        string codigo = "";

        public void codigoProveedor(String cod)
        {
            codigo = cod;

            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaProducto = new MySqlCommand("SELECT * FROM proveedores WHERE Codigo = " + codigo, conexionBBDD);
                reader = consultaProducto.ExecuteReader();
                reader.Read();
                if (!reader.IsDBNull(0))
                {
                    TB_NombreEmpresa.Text = reader.GetString(1);
                    TB_CIF_Proveedor.Text = reader.GetString(2);
                }
                reader.Close();
                conexionBBDD.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string query = "UPDATE proveedores SET Nombre_Empresa = '" + TB_NombreEmpresa.Text + "',CIF='" + TB_CIF_Proveedor.Text + "' WHERE Codigo = " + codigo;
            if (!TB_NombreEmpresa.Text.Equals(String.Empty) && !TB_CIF_Proveedor.Text.Equals(String.Empty))
            {
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    conexionBBDD.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conexionBBDD);

                    cmd.ExecuteNonQuery();

                    conexionBBDD.Close();
                    Ventana_listado_Proveedores.v.DatosDataGridProveedores();
                    lbl_ERROR.Foreground = Brushes.Green;
                    lbl_ERROR.Content = "PROVEEDOR MODIFICADO CORRECTAMENTE";
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDireccion_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Proveedores_Modificar_Direccion ventana_Proveedores_Modificar_Direccion = new Ventana_Proveedores_Modificar_Direccion();
            ventana_Proveedores_Modificar_Direccion.DatosProveedores(codigo);
            ventana_Proveedores_Modificar_Direccion.Show();
        }

        private void btnEmail_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Proveedores_Modificar_Email ventana_Proveedores_Modificar_Email = new Ventana_Proveedores_Modificar_Email();
            ventana_Proveedores_Modificar_Email.DatosProveedores(codigo);
            ventana_Proveedores_Modificar_Email.Show();
        }

        private void btnTelefono_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Proveedores_Modificar_Telefono ventana_Proveedores_Modificar_Telefono = new Ventana_Proveedores_Modificar_Telefono();
            ventana_Proveedores_Modificar_Telefono.DatosProveedores(codigo);
            ventana_Proveedores_Modificar_Telefono.Show();
        }

        private void TB_CIF_Proveedor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 48 && ascci <= 57) e.Handled = false;

            else e.Handled = true;
        }
    }
}
