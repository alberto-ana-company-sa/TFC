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
    /// Lógica de interacción para Ventana_Cliente_Anadir.xaml
    /// </summary>
    public partial class Ventana_Cliente_Anadir : Window
    {
        public Ventana_Cliente_Anadir()
        {
            InitializeComponent();
        }

        Boolean btn_Telefono = false;
        Boolean btn_Email = false;
        Boolean btn_Direccion = false;
        Boolean btn_Guardar = false;

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(!TB_NombreCliente.Text.Equals(String.Empty) && !TB_Apellido1_Cliente.Text.Equals(String.Empty) && !TB_Apellido2_Cliente.Text.Equals(String.Empty) && !TB_DNI_Cliente.Text.Equals(String.Empty))
            {
                btn_Guardar = true;

                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    conexionBBDD.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO clientes ( Nombre, Apellido1, Apellido2, DNI) VALUES ('" + TB_NombreCliente.Text + "' , '" + TB_Apellido1_Cliente.Text + "', '" + TB_Apellido2_Cliente.Text + "', '" + TB_DNI_Cliente.Text + "');", conexionBBDD);

                    cmd.ExecuteNonQuery();

                    conexionBBDD.Close();
                    btnDireccion.IsEnabled = true;
                    btnTelefono.IsEnabled = true;
                    btnEmail.IsEnabled = true;
                    Ventana_listado_Clientes.v.DataGridClientes();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnEmail_Click(object sender, RoutedEventArgs e)
        {
            btn_Email = true;
            string codigo = BuscarCodigo();
            Ventana_Cliente_Email ventana_Cliente_Email = new Ventana_Cliente_Email();
            ventana_Cliente_Email.TextBoxNombreCliente(TB_NombreCliente.Text, codigo);
            ventana_Cliente_Email.Show();

        }

        private void btnTelefono_Click(object sender, RoutedEventArgs e)
        {
            btn_Telefono = true;
            Ventana_Cliente_Telefono ventana_Cliente_Telefono = new Ventana_Cliente_Telefono();
            string codigo = BuscarCodigo();
            ventana_Cliente_Telefono.TextBoxNombreCliente(TB_NombreCliente.Text, codigo);
            ventana_Cliente_Telefono.Show();

        }

        public String BuscarCodigo()
        {
            String codigo = "";
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaCodigo = new MySqlCommand("SELECT Codigo FROM clientes WHERE Nombre ='" + TB_NombreCliente.Text + "' AND Apellido1 ='" + TB_Apellido1_Cliente.Text + "' " +
                    "AND Apellido2 ='" + TB_Apellido2_Cliente.Text + "' AND DNI ='" + TB_DNI_Cliente.Text + "'"
                    , conexionBBDD);
                reader = consultaCodigo.ExecuteReader();


                while (reader.Read())
                {
                    codigo = reader.GetString(0);
                }

                reader.Close();
                conexionBBDD.Close();
                return codigo;
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return "";
        }

        private void btnDireccion_Click(object sender, RoutedEventArgs e)
        {
            btn_Direccion = true;
            Ventana_Cliente_Direccion ventana_Cliente_Direccion = new Ventana_Cliente_Direccion();
            string codigo = BuscarCodigo();
            ventana_Cliente_Direccion.TextBoxNombreCliente(TB_NombreCliente.Text, codigo);
            ventana_Cliente_Direccion.Show();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (btn_Direccion == true && btn_Email == true && btn_Telefono == true)
            {
                this.Close();
            }
            else if (!btn_Guardar)
            {
                this.Close();
            }
            else
            {
                lbl_ERROR.Content = "OBLIGATORIO AGREGAR TELEFONO, DIRECCION Y EMAIL";
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
    }
}
