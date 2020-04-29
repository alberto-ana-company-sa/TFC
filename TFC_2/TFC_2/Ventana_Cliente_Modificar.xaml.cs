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
    /// Lógica de interacción para Ventana_Cliente_Modificar.xaml
    /// </summary>
    public partial class Ventana_Cliente_Modificar : Window
    {
        public Ventana_Cliente_Modificar()
        {
            InitializeComponent();
        }

        String codigo_cliente = "";

        public void codigoCliente(String codigo)
        {
            codigo_cliente = codigo;

            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaProducto = new MySqlCommand("SELECT * FROM clientes WHERE Codigo = " + codigo_cliente, conexionBBDD);
                reader = consultaProducto.ExecuteReader();
                reader.Read();
                if (!reader.IsDBNull(0))
                {
                    TB_NombreCliente.Text = reader.GetString(1);
                    TB_Apellido1_Cliente.Text = reader.GetString(2);
                    TB_Apellido2_Cliente.Text = reader.GetString(3);
                    TB_DNI_Cliente.Text = reader.GetString(4);
                }
                reader.Close();
                conexionBBDD.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void TB_NombreCliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122)

                e.Handled = false;

            else e.Handled = true;
        }

        private void TB_Apellido1_Cliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122)

                e.Handled = false;

            else e.Handled = true;
        }

        private void TB_Apellido2_Cliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122)

                e.Handled = false;

            else e.Handled = true;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string query = "UPDATE clientes SET Nombre = '" + TB_NombreCliente.Text + "',Apellido1='" + TB_Apellido1_Cliente.Text + "', Apellido2 ='" + TB_Apellido2_Cliente.Text + "',DNI='" + TB_DNI_Cliente.Text + "' WHERE Codigo = " + codigo_cliente;
            if (!TB_NombreCliente.Text.Equals(String.Empty) && !TB_Apellido1_Cliente.Text.Equals(String.Empty) && !TB_Apellido2_Cliente.Text.Equals(String.Empty) && !TB_DNI_Cliente.Text.Equals(String.Empty))
            {
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    conexionBBDD.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conexionBBDD);

                    cmd.ExecuteNonQuery();

                    conexionBBDD.Close();
                    Ventana_listado_Clientes.v.DataGridClientes();
                    lbl_ERROR.Foreground = Brushes.Green;
                    lbl_ERROR.Content = "CLIENTE MODIFICADO CORRECTAMENTE";
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnEmail_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Cliente_Modificar_Email ventana_Cliente_Modificar_Email = new Ventana_Cliente_Modificar_Email();
            ventana_Cliente_Modificar_Email.DatosCliente(codigo_cliente);
            ventana_Cliente_Modificar_Email.Show();
        }

        private void btnTelefono_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Cliente_Modificar_Telefono ventana_Cliente_Modificar_Telefono = new Ventana_Cliente_Modificar_Telefono();
            ventana_Cliente_Modificar_Telefono.DatosCliente(codigo_cliente);
            ventana_Cliente_Modificar_Telefono.Show();
        }

        private void btnDireccion_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Cliente_Modificar_Direccion ventana_Cliente_Modificar_Direccion = new Ventana_Cliente_Modificar_Direccion();
            ventana_Cliente_Modificar_Direccion.DatosCliente(codigo_cliente);
            ventana_Cliente_Modificar_Direccion.Show();
        }
    }
}
